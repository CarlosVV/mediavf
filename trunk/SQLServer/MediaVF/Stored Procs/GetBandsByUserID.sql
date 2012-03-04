IF EXISTS (
	SELECT *
	FROM sys.procedures
	WHERE [name] = 'GetBandsByUserID'
)
BEGIN
	DROP PROCEDURE [dbo].[GetBandsByUserID]
END
GO

CREATE PROCEDURE [dbo].[GetBandsByUserID]
(
	@UserID			int			= NULL
)
AS
BEGIN
	SELECT
		*
	FROM
		[dbo].[Band] b
	INNER JOIN [dbo].[UserBand] ub ON
		b.[ID] = ub.[BandID]
	WHERE
		ub.[UserID] = @UserID
END
GO

GRANT EXECUTE ON [dbo].[GetBandsByUserID] TO [MediaVFServiceUser]