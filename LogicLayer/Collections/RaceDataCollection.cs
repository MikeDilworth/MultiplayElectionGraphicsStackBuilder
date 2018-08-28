using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataInterface.DataModel;
using DataInterface.DataAccess;
using System.Data.SqlClient;
using System.ComponentModel;

namespace LogicLayer.Collections
{

    public class RaceDataCollection
    {
        #region Logger instantiation - uses reflection to get module name
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties and Members
        public BindingList<RaceDataModel> raceData;
        public string ElectionsDBConnectionString { get; set; }

        private int collectionCount { get; set; }
        public int CollectionCount
        {
            get { return this.collectionCount; }
            set { this.collectionCount = value; }
        }
        #endregion

        #region Public Methods
        // Constructor - instantiates list collection
        public RaceDataCollection()
        {
            // Create list of candidate data for specified race
            raceData = new BindingList<RaceDataModel>();
        }

        /// <summary>
        /// Get the MSE Stack elements list from the SQL DB; clears out existing collection first
        /// </summary>
        public BindingList<RaceDataModel> GetRaceDataCollection(Int16 stateNumber, string raceOffice, Int16 cd, string electionType, Int16 candidatesToReturn)
        {
            DataTable dataTable;

            // Clear out the current collection
            raceData.Clear();

            try
            {
                RaceDataAccess raceDataAccess = new RaceDataAccess();
                raceDataAccess.ElectionsDBConnectionString = ElectionsDBConnectionString;
                // If state ID = -1 => not an actual data request - just initializing collection
                if (stateNumber != -1)
                {
                    dataTable = raceDataAccess.GetRaceData(stateNumber, raceOffice, cd, electionType);

                    // Init counter & increment
                    Int16 candidateCount = 0;
                    
                    foreach (DataRow row in dataTable.Rows)
                    {
                        // Dump out if we've hit the number of candidates required
                        if (candidatesToReturn >= 1) 
                            if (candidateCount > candidatesToReturn)
                        {
                            break;
                        }

                        var newRaceCandidateData = new RaceDataModel()
                        {
                            // Data for 1 candidate in the race
                            Office = row["ofc"].ToString() ?? "",
                            OfficeName = row["ofcName"].ToString() ?? "",
                            StateName = row["jName"].ToString() ?? "",
                            StateAbbv = row["stateAbbv"].ToString() ?? "",
                            IsAtLargeHouseState = Convert.ToBoolean(row["IsAtLargeHouseState"] ?? 0),
                            CD = Convert.ToInt16(row["jCde"] ?? 0),
                            TotalPrecincts = Convert.ToInt32(row["totPcts"] ?? 0),
                            PrecinctsReporting = Convert.ToInt32(row["pctsRep"] ?? 0),
                            PercentExpectedVote = Convert.ToSingle(row["pctExpVote"] ?? 0),
                            CandidateID = Convert.ToInt32(row["cID"] ?? 0),
                            FoxID = row["FoxID"].ToString() ?? "",
                            CandidateLastName = row["candLastName"].ToString() ?? "",
                            LastNameAir = row["LastNameAir"].ToString() ?? "",
                            CandidateFirstName = row["candFirstName"].ToString() ?? "",
                            UseHeadshotFNC = Convert.ToBoolean(row["UseHeadshot"] ?? 0),
                            HeadshotPathFNC = row["HeadshotPath"].ToString() ?? "",
                            UseHeadshotFBN = Convert.ToBoolean(row["UseHeadshot_FBN"] ?? 0),
                            HeadshotPathFBN = row["HeadshotPath_FBN"].ToString() ?? "",
                            CandidatePartyID = row["majorPtyID"].ToString() ?? "",
                            cStat = row["cStat"].ToString() ?? "",
                            InIncumbentPartyFlag = row["inIncPtyFlg"].ToString() ?? "",
                            IsIncumbentFlag = row["isIncFlg"].ToString() ?? "",
                            CandidateVoteCount = Convert.ToInt32(row["cVote"] ?? 0),
                            TotalVoteCount = Convert.ToInt32(row["voteSum"] ?? 0),
                            RaceWinnerCalled = Convert.ToBoolean(row["Race_WinnerCalled"] ?? 0),
                            RaceWinnerCallTime = Convert.ToDateTime(row["Race_WinnerCallTime"] ?? 0),
                            RaceTooCloseToCall = Convert.ToBoolean(row["Race_TooCloseToCall"] ?? 0),
                            RaceWinnerCandidateID = Convert.ToInt32(row["Race_WinnerCandidateID"] ?? 0),
                            RacePollClosingTime = Convert.ToDateTime(row["Race_PollClosingTime_DateTime"] ?? 0),
                            RaceUseAPRaceCall = Convert.ToBoolean(row["Use_AP_Race_Call"] ?? 0),
                        };
                        raceData.Add(newRaceCandidateData);
                        candidateCount += 1;

                        this.collectionCount = raceData.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("RaceDataCollection Exception occurred: " + ex.Message);
                log.Debug("RaceDataCollection Exception occurred", ex);
            }
            // Return 
            return raceData;
        }

        #endregion
    }
}
