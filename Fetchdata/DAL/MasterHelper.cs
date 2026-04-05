using Dapper;
using Model.DTO;
using Model.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using static Mono.Security.X509.X520;

namespace DAL
{
    public class MasterHelper : BaseDBHelper
    {
        private IConfig _config;
        string _log = "";
        String Database = "adminboundary";
        //String Database = "labour_app";

        public MasterHelper(IConfig conifg) : base(conifg)
        {
            _config = conifg;
            _log = conifg.Log;


        }


        public async Task<Response<IEnumerable<dynamic>>> GetDesignation(DesignationEntity webLogin)
        {
            Response<IEnumerable<dynamic>> objResponse = new Response<IEnumerable<dynamic>>();
            string connectionString = $"{_config.ConnectionString}{Database}";
            _con = new Npgsql.NpgsqlConnection(connectionString);
            try
            {
                _con.Open();
                string querytype = "GetDesignation";
                string[] param = { };
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@querytype", querytype);
                parameters.Add("@param_array", param);
                IDbTransaction dbTransactiondbTransaction = _con.BeginTransaction();
                objResponse.Data = await _con.QueryAsync<dynamic>("gis_db_access_users.fn_get_general_multipurpose", param: parameters, dbTransactiondbTransaction, commandType: CommandType.StoredProcedure);
                dbTransactiondbTransaction.Commit(); 
                _con.Close();
                if (objResponse.Data != null && objResponse.Data.Count() > 0)
                {
                    objResponse.IsSuccess = true;
                    objResponse.Message = "Designation retrieved successfully";
                    objResponse.DataLength = objResponse.Data.Count();
                }
                else
                {
                    objResponse.IsSuccess = false;
                    objResponse.Message = "Records Not Found";
                    objResponse.DataLength = 0;
                }
                return objResponse;

            }
            catch (Exception ex)
            {
                Utility.LogService.WriteErrorLog("GetDesignation", ex.Message, _log);
                objResponse.IsSuccess = false;
                objResponse.Message = ex.Message;
            }
            return null;
        }

        

        public async Task<Response<IEnumerable<dynamic>>> sendsmstocandidate(SMSEntity _InputParam)
        {
            Response<IEnumerable<dynamic>> objResponse = new Response<IEnumerable<dynamic>>();
            int startRank = _InputParam.from;
            int endRank = _InputParam.to;
            int defaultRank = _InputParam.other;
            int count = 0;
            string connectionString = $"{_config.ConnectionString}{Database}";
            _con = new Npgsql.NpgsqlConnection(connectionString);
            //var candidates = new List<Candidate>();
            try
            {
                _con.Open();
                var queryBuilder = new StringBuilder("SELECT full_name, mobile_number, scheduled_date FROM gis_db_access_users.candidate WHERE sms_send = 0");

                if (startRank > 0)
                    queryBuilder.Append(" AND c_rank >= @StartRank");

                if (endRank > 0)
                    queryBuilder.Append(" AND c_rank <= @EndRank");

                if (defaultRank > 0)
                    queryBuilder.Append(" OR c_rank = @DefaultRank");

                using (var cmd = new NpgsqlCommand(queryBuilder.ToString(), _con))
                {
                    if (startRank > 0)
                        cmd.Parameters.AddWithValue("@StartRank", startRank);
                    if (endRank > 0)
                        cmd.Parameters.AddWithValue("@EndRank", endRank);
                    if (defaultRank > 0)
                        cmd.Parameters.AddWithValue("@DefaultRank", defaultRank);

                    
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);  // Fill the DataTable with the data

                        foreach (DataRow row in dt.Rows)
                        {
                            //string round = "Interview date reschedule";
                            string round = "Final Opportunity Round";
                            string date =  row["scheduled_date"].ToString();
                            string message = "MPSEDC Campus Recruitment Drive - " + round + ". Congrats " + row["full_name"].ToString() + "! You've been selected for the 2nd round on " + date + " Last Chance for Absent Candidates. Details & documents shared via email/WhatsApp. Best Regards, MPSEDC HR Team";
                            string temp = "1307173875914507163";
                            SmsService smsService = new SmsService();

                            // Store the mobile number to use later
                            string mobileNumber = row["mobile_number"].ToString();

                            try
                            {
                                // Send SMS
                                var response = smsService.sendSingleSMS(mobileNumber, message, temp);
                                //returntype += response;
                                count++;

                                // Perform update operation on the same open connection
                                if (response.Contains("402"))
                                {
                                    var query = "UPDATE gis_db_access_users.candidate SET sms_send = 1 WHERE mobile_number = @MobileNumber";
                                    using (var cmd1 = new NpgsqlCommand(query, _con))
                                    {
                                        cmd1.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                                        cmd1.ExecuteNonQuery();
                                    }

                                }
                                
                            }
                            catch (Exception ex)
                            {
                                Utility.LogService.WriteErrorLog("GetCandidates", ex.Message, _log);
                            }
                        }
                        objResponse.ResponseCode = 200;
                        objResponse.DataLength = count;
                        objResponse.Message = "Message send successfully";
                        objResponse.IsSuccess = true;
                    }
                }
                return objResponse;

            }
            catch (Exception ex) {
                Utility.LogService.WriteErrorLog("GetCandidates", ex.Message, _log);
            }
            finally
            {
                _con.Close();
            }
            return null;


        }


        private void UpdateSmsStatus(String MobileNumber)
        {

            string connectionString = $"{_config.ConnectionString}{Database}";

            try
            {
                using (var conn = new Npgsql.NpgsqlConnection(connectionString))
                {
                    conn.Open(); // Open connection only when needed

                    var query = "UPDATE gis_db_access_users.candidate SET sms_send = 1 WHERE mobile_number = @MobileNumber";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MobileNumber", MobileNumber);
                        cmd.ExecuteNonQuery();
                    }
                } // Connection is automatically closed here
            }
            catch (Exception ex)
            {
                Utility.LogService.WriteErrorLog("UpdateSmsStatus", ex.Message, _log);
            }
            //string connectionString = $"{_config.ConnectionString}{Database}";
            //_con = new Npgsql.NpgsqlConnection(connectionString);
            ////var candidates = new List<Candidate>();
            //try
            //{
            //    _con.Open();

            //    //foreach (var candidate in candidates)
            //    //{
            //        var query = "UPDATE gis_db_access_users.candidate SET sms_send = 1 WHERE mobile_number = @MobileNumber";
            //        using (var cmd = new NpgsqlCommand(query, _con))
            //        {
            //            cmd.Parameters.AddWithValue("@MobileNumber", MobileNumber);
            //            cmd.ExecuteNonQuery();
            //        }
            //    //}

            //}
            //catch(Exception ex) {
            //    Utility.LogService.WriteErrorLog("UpdateSmsStatus", ex.Message, _log);

            //}
        }
        public async Task<Response<IEnumerable<dynamic>>> GetListOfCandidate(CandidateEntity webLogin)
        {
            Response<IEnumerable<dynamic>> objResponse = new Response<IEnumerable<dynamic>>();
            string connectionString = $"{_config.ConnectionString}{Database}";
            _con = new Npgsql.NpgsqlConnection(connectionString);
            try
            {
                _con.Open();
                string querytype = "GetListOfCandidate";
                string[] param = { webLogin.from.ToString(), webLogin.to.ToString(), webLogin.smsflg.ToString() };
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@querytype", querytype);
                parameters.Add("@param_array", param);
                IDbTransaction dbTransactiondbTransaction = _con.BeginTransaction();
                objResponse.Data = await _con.QueryAsync<dynamic>("gis_db_access_users.fn_get_general_multipurpose", param: parameters, dbTransactiondbTransaction, commandType: CommandType.StoredProcedure);
                dbTransactiondbTransaction.Commit();
                _con.Close();
                if (objResponse.Data != null && objResponse.Data.Count() > 0)
                {
                    objResponse.IsSuccess = true;
                    objResponse.Message = "Candidate retrieved successfully";
                    objResponse.DataLength = objResponse.Data.Count();
                }
                else
                {
                    objResponse.IsSuccess = false;
                    objResponse.Message = "Records Not Found";
                    objResponse.DataLength = 0;
                }
                return objResponse;

            }
            catch (Exception ex)
            {
                Utility.LogService.WriteErrorLog("GetListOfCandidate", ex.Message, _log);
                objResponse.IsSuccess = false;
                objResponse.Message = ex.Message;
            }
            return null;
        }
    }

    public class Candidate
    {
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public DateTime ScheduledDate { get; set; }
    }
}
