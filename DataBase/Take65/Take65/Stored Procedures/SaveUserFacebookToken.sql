CREATE PROCEDURE SaveUserFacebookToken
	@UserId NUMERIC(18, 0) = NULL,
	@FacebookTokenShort NVARCHAR(500),
	@FacebookTokenLong NVARCHAR(500),
	@FacebookTokenLongExpires BIGINT,
	@FacebookTokenLongExpiresDate DATETIME = NULL
AS
	UPDATE [User]
		SET 
		 [UserFacebookTokenLong] = @FacebookTokenLong
		,[UserFacebookTokenShort] = @FacebookTokenShort
		,[UserFacebookTokenLongExpires] = @FacebookTokenLongExpires
		,[UserFacebookTokenLongExpiresDate] = @FacebookTokenLongExpiresDate
		,lastupdate = GETDATE()
		WHERE UserId = @UserId