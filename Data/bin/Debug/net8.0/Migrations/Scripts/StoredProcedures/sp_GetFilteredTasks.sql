CREATE OR ALTER PROCEDURE sp_GetFilteredTasks
    @SearchTerm NVARCHAR(200) = NULL,
    @Status INT = NULL,
    @Priority INT = NULL,
    @SortColumn NVARCHAR(50) = 'CreatedAt',
    @SortOrder NVARCHAR(4) = 'ASC',
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @UserId INT = NULL,
    @IsAdmin BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    -- 🛡️ Safe fallback for columns
    IF @SortColumn NOT IN ('Title', 'Status', 'Priority', 'DueDate', 'UpdatedAt', 'CreatedAt')
        SET @SortColumn = 'CreatedAt';

    IF @SortOrder NOT IN ('ASC', 'DESC')
        SET @SortOrder = 'ASC';

    DECLARE @Sql NVARCHAR(MAX) = '
    SELECT 
        t.Id,
        t.Title,
        t.Description,
        t.Status,
        t.Priority,
        t.DueDate,
        t.CreatedAt,
        t.UpdatedAt,
        u1.UserName AS CreatorName,
        u2.UserName AS AssigneeName
    FROM Tasks t
    INNER JOIN Users u1 ON t.CreatorId = u1.Id
    INNER JOIN Users u2 ON t.AssigneeId = u2.Id
    WHERE t.IsDeleted = 0';

    IF @SearchTerm IS NOT NULL
        SET @Sql += ' AND (t.Title LIKE ''%' + @SearchTerm + '%'' OR t.Description LIKE ''%' + @SearchTerm + '%'')';

    IF @Status IS NOT NULL
        SET @Sql += ' AND t.Status = ' + CAST(@Status AS NVARCHAR);

    IF @Priority IS NOT NULL
        SET @Sql += ' AND t.Priority = ' + CAST(@Priority AS NVARCHAR);

    IF @IsAdmin = 0
        SET @Sql += ' AND ((t.CreatorId = @UserId AND t.AssigneeId = @UserId) OR t.AssigneeId = @UserId)';
    ELSE
        SET @Sql += ' AND NOT (t.CreatorId = t.AssigneeId AND t.AssigneeId != @UserId)';


    -- ✅ Safe & Valid ORDER BY
    SET @Sql += ' ORDER BY [' + @SortColumn + '] ' + @SortOrder;
    SET @Sql += ' OFFSET ' + CAST(@Offset AS NVARCHAR) + ' ROWS FETCH NEXT ' + CAST(@PageSize AS NVARCHAR) + ' ROWS ONLY;';

    -- PRINT @Sql; -- (for debug)
    EXEC sp_executesql @Sql;
END;
