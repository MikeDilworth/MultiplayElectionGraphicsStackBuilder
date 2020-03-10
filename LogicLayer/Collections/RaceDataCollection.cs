﻿using System;
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
        //public BindingList<RaceDataModel> GetRaceDataCollection(Int16 stateNumber, string raceOffice, Int16 cd, string electionType, Int16 candidatesToReturn)
        public BindingList<RaceDataModel> GetRaceDataCollection(Int16 stateNumber, string raceOffice, Int16 cd, string electionType, Int16 candidatesToReturn,
            bool candidateSelectEnable, int candidateId1, int candidateId2, int candidateId3, int candidateId4)
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
                    dataTable = raceDataAccess.GetRaceData(stateNumber, raceOffice, cd, electionType, candidateSelectEnable, candidateId1, candidateId2, candidateId3, candidateId4);

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
                            Office = row["ofc"].ToString().Trim() ?? "",
                            OfficeName = row["ofcName"].ToString().Trim() ?? "",
                            StateName = row["jName"].ToString().Trim() ?? "",
                            StateAbbv = row["stateAbbv"].ToString().Trim() ?? "",
                            IsAtLargeHouseState = Convert.ToBoolean(row["IsAtLargeHouseState"] ?? 0),
                            CD = Convert.ToInt16(row["jCde"] ?? 0),
                            eType = row["eType"].ToString().Trim() ?? "",
                            TotalPrecincts = Convert.ToInt32(row["totPcts"] ?? 0),
                            PrecinctsReporting = Convert.ToInt32(row["pctsRep"] ?? 0),
                            PercentExpectedVote = Convert.ToSingle(row["pctExpVote"] ?? 0),
                            // Added 03/10/2020 to support 2020 primaries
                            DemDelegatesAtStake = Convert.ToInt16(row["DemDelegatesAvailable"] ?? 0),
                            RepDelegatesAtStake = Convert.ToInt16(row["RepDelegatesAvailable"] ?? 0),
                            CandidateID = Convert.ToInt32(row["cID"] ?? 0),
                            FoxID = row["FoxID"].ToString().Trim() ?? "USGOV999999",
                            CandidateLastName = row["candLastName"].ToString().Trim() ?? "",
                            LastNameAir = row["LastNameAir"].ToString().Trim() ?? "",
                            CandidateFirstName = row["candFirstName"].ToString() ?? "",
                            UseHeadshotFNC = Convert.ToBoolean(row["UseHeadshot"] ?? 0),
                            HeadshotPathFNC = row["HeadshotPath"].ToString().Trim() ?? "",
                            UseHeadshotFBN = Convert.ToBoolean(row["UseHeadshot_FBN"] ?? 0),
                            HeadshotPathFBN = row["HeadshotPath_FBN"].ToString().Trim() ?? "",
                            CandidatePartyID = row["majorPtyID"].ToString().Trim() ?? "",
                            cStat = row["cStat"].ToString().Trim() ?? "",
                            estTS = row["estTS"].ToString().Trim(),
                            InIncumbentPartyFlag = row["inIncPtyFlg"].ToString().Trim() ?? "",
                            IsIncumbentFlag = row["isIncFlg"].ToString().Trim() ?? "",
                            CandidateVoteCount = Convert.ToInt32(row["cVote"] ?? 0),
                            TotalVoteCount = Convert.ToInt32(row["voteSum"] ?? 0),
                            RaceWinnerCalled = Convert.ToBoolean(row["Race_WinnerCalled"] ?? 0),
                            RaceWinnerCallTime = Convert.ToDateTime(row["Race_WinnerCallTime"] ?? 0),
                            RaceTooCloseToCall = Convert.ToBoolean(row["Race_TooCloseToCall"] ?? 0),
                            RaceWinnerCandidateID = Convert.ToInt32(row["Race_WinnerCandidateID"] ?? 0),
                            RacePollClosingTime = Convert.ToDateTime(row["Race_PollClosingTime_DateTime"] ?? 0),
                            RaceUseAPRaceCall = Convert.ToBoolean(row["Use_AP_Race_Call"] ?? 0),
                            RaceIgnoreGain = Convert.ToBoolean(row["IgnoreGain"] ?? 0),
                        };
                        if (newRaceCandidateData.FoxID.Length < 10)
                            newRaceCandidateData.FoxID = "USGOV999999";
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
