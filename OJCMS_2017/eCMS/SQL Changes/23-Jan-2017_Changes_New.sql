USE [LIVE_Nov_21_2016]
GO


/****** Object:  UserDefinedFunction [dbo].[GetLatNotesByCaseID]    Script Date: 2/6/2017 4:01:10 PM ******/
DROP FUNCTION [dbo].[GetLatNotesByCaseID]
GO

/****** Object:  UserDefinedFunction [dbo].[GetLatNotesByCaseID]    Script Date: 2/6/2017 4:01:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<The One Team>
-- Create date: <23-Jan-2017, Monday>
-- Description:	<Get Latest note by case id>
-- =============================================
CREATE FUNCTION [dbo].[GetLatNotesByCaseID]
(
	@ID INT
)
RETURNS varchar(MAX)
AS
BEGIN

DECLARE @Note VARCHAR(MAX)

--Select Top 1 @Note = Note from CaseProgressNote CP
--LEFT JOIN CaseProgressNoteMembers CM ON CM.CaseProgressNoteID = CP.ID
--inner JOIN CaseMember CAM ON CAM.ID = CP.CaseMemberID OR CAM.ID = CP.CaseMemberID
--WHERE CAM.CaseID=7538
--order by CP.ID 

SELECt Top 1 @Note = Note FROM CaseProgressNote
INNER JOIN CaseProgressNoteMembers ON CaseProgressNoteMembers.CaseProgressNoteID = CaseProgressNote.ID
INNER JOIN CaseMember ON CaseMember.ID = CaseProgressNoteMembers.CaseMemberID OR CaseProgressNote.CaseMemberID = CaseMember.ID
WHERE CaseMember.CaseID = @ID
order by CaseProgressNote.ID 

RETURN @Note 

END

GO


/****** Object:  Index [idx_CaseProgressNote_CaseMemberID]    Script Date: 1/24/2017 7:40:01 PM ******/
CREATE NONCLUSTERED INDEX [idx_CaseProgressNote_CaseMemberID] ON [dbo].[CaseProgressNote]
(
	[CaseMemberID] ASC
)
INCLUDE ( 	[ID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


