﻿CREATE PROCEDURE [dbo].[Tegalize]
	@Id NUMERIC(18, 0) = NULL,
	@TEXT VARCHAR(1000)
AS

DECLARE @ARRAY VARCHAR(MAX), @DELIMITER VARCHAR(100), @S VARCHAR(MAX)

SELECT @ARRAY = ' ' + @TEXT + ' '
SELECT @ARRAY =  REPLACE(@ARRAY,',','')
SELECT @ARRAY =  REPLACE(@ARRAY,'-',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,'´','')
SELECT @ARRAY =  REPLACE(@ARRAY,'`','')
SELECT @ARRAY =  REPLACE(@ARRAY,';','')
SELECT @ARRAY =  REPLACE(@ARRAY,'...','')
SELECT @ARRAY =  REPLACE(@ARRAY,':','')
SELECT @ARRAY =  REPLACE(@ARRAY,'''','')
SELECT @ARRAY =  REPLACE(@ARRAY,' C/ ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' P/ ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' E ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' / ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' \ ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' ( ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' ) ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' A ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' ABLE ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' ABOUT ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' ACROSS ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' AFTER ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' ALL ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' ALMOST ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' ALSO ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' AM ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' AMONG ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' AN ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' AND ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' ANY ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' ARE ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' AS ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' AT ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' BE ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' BECAUSE ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' BEEN ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' BUT ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' BY ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' CAN ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' CANNOT ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' COULD ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' DEAR ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' DID ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' DO ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' DOES ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' EITHER ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' ELSE ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' EVER ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' EVERY ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' FOR ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' FROM ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' GET ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' GOT ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' HAD ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' HAS ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' HAVE ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' HE ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' HER ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' HERS ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' HIM ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' HIS ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' HOW ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' HOWEVER ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' I ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' IF ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' IN ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' INTO ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' IS ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' IT ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' ITS ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' JUST ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' LEAST ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' LET ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' LIKE ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' LIKELY ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' MAY ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' ME ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' MIGHT ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' MOST ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' MUST ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' MY ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' NEITHER ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' NO ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' NOR ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' NOT ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' OF ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' OFF ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' OFTEN ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' ON ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' ONLY ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' OR ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' OTHER ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' OUR ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' OWN ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' RATHER ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' SAID ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' SAY ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' SAYS ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' SHE ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' SHOULD ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' SINCE ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' SO ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' SOME ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' THAN ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' THAT ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' THE ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' THEIR ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' THEM ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' THEN ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' THERE ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' THESE ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' THEY ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' THIS ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' TIS ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' TO ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' TOO ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' TWAS ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' US ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' WANTS ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' WAS ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' WE ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' WERE ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' WHAT ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' WHEN ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' WHERE ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' WHICH ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' WHILE ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' WHO ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' WHOM ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' WHY ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' WILL ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' WITH ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' WOULD ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' YET ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' YOU ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' YOUR ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' ! ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' ? ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' * ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' : ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,' & ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,'   ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,'  ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,'  ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,'  ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,'  ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,'  ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,'  ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,'  ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,'  ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,'  ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,'  ',' ')
SELECT @ARRAY =  REPLACE(@ARRAY,'  ',' ')


SELECT @DELIMITER = ' '

IF LEN(@ARRAY) > 0 SET @ARRAY = @ARRAY + @DELIMITER 

BEGIN
	WHILE LEN(@ARRAY) > 0
	BEGIN
	   
	   SET @ARRAY = [dbo].[RemoveHtml](@ARRAY)
	   
	   IF UPPER([dbo].[NormalizeChar](RTRIM(LTRIM(SUBSTRING(@ARRAY, 1, CHARINDEX(@DELIMITER, @ARRAY) - 1))))) <> ' '	
	   BEGIN
	   
		   INSERT INTO [dbo].[FeedContentTag] (
			[FeedContentId]
			,[FeedContentTagNormalized]
			,[FeedContentTagDisplay]
			)
			SELECT @Id
				,UPPER([dbo].[NormalizeChar](RTRIM(LTRIM(SUBSTRING(@ARRAY, 1, CHARINDEX(@DELIMITER, @ARRAY) - 1)))))
				,RTRIM(LTRIM(SUBSTRING(@ARRAY, 1, CHARINDEX(@DELIMITER, @ARRAY) - 1)))
	   END
			
	   SELECT @ARRAY = SUBSTRING(@ARRAY, CHARINDEX(@DELIMITER, @ARRAY) + 1, LEN(@ARRAY))
	END
END