CREATE PROCEDURE GetUserFacebookFriends
	@Id NUMERIC(18, 0),
	@ReturnXml XML OUTPUT
AS
BEGIN
	SET @ReturnXml = (
	SELECT   [FacebookFriendId] AS 'Id'
			,[FacebookFriendName] AS 'Name'
			,[FacebookPhotoCount] AS 'PhotoCount'
		FROM UserFacebookFriend
			WHERE UserId = @Id
		FOR XML PATH('FacebookProfile'), TYPE, ROOT('ArrayOfFacebookProfile'))
END