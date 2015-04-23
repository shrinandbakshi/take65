CREATE PROCEDURE UserFacebookFriendClean
	@UserId NUMERIC(18, 0),
	@FacebookFriendList NVARCHAR(500) = NULL
AS
IF (@FacebookFriendList IS NOT NULL)
BEGIN
	EXEC('DELETE UserFacebookFriend WHERE UserId = ' + @UserId + ' AND FacebookFriendId NOT IN (' + @FacebookFriendList + ')')
END
ELSE
BEGIN
	DELETE UserFacebookFriend WHERE UserId = @UserId
END