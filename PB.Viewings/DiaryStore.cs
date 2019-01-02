using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PB.Viewings
{
    public class DiaryStore
    {
        public static Diary FindAVLPEDiary(int advertId, int year, int month, int day)
        {
            var slots = new List<Slot>();
            
            var connectionString = ConfigurationManager.ConnectionStrings["DiaryDatbse.ConnectionString"].ConnectionString;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                var command = new SqlCommand("sp_getLpeDiary", sqlConnection) { CommandType = CommandType.StoredProcedure };
                var yearIn = command.Parameters.Add("@year", SqlDbType.Int);
                yearIn.Direction = ParameterDirection.Input;
                yearIn.Value = year;
                
                var monthIn = command.Parameters.Add("@month", SqlDbType.Int);
                monthIn.Direction = ParameterDirection.Input;
                monthIn.Value = month;
                
                var dayIN = command.Parameters.Add("@day", SqlDbType.Int);
                dayIN.Direction = ParameterDirection.Input;
                dayIN.Value = day;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var slot = new Slot() { IsBooked = reader.GetBoolean(0), StartTime = reader.GetDateTime(1) };

                        slots.Add(slot);
                    }
                }
            }
            
            return new Diary()
            {
                Slots = slots
            };
        }

        public static Diary FindCustomerDiary(int propertyId, int year, int month, int day)
        {
            var slots = new List<Slot>();
            
            var connectionString = ConfigurationManager.ConnectionStrings["DiaryDatbse.ConnectionString"].ConnectionString;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                var command = new SqlCommand("sp_getCustomerDiary", sqlConnection) { CommandType = CommandType.StoredProcedure };
                var yearIn = command.Parameters.Add("@year", SqlDbType.Int);
                yearIn.Direction = ParameterDirection.Input;
                yearIn.Value = year;
                
                var monthIn = command.Parameters.Add("@month", SqlDbType.Int);
                monthIn.Direction = ParameterDirection.Input;
                monthIn.Value = month;
                
                var dayIN = command.Parameters.Add("@day", SqlDbType.Int);
                dayIN.Direction = ParameterDirection.Input;
                dayIN.Value = day;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var slot = new Slot() { IsBooked = reader.GetBoolean(0), StartTime = reader.GetDateTime(1) };

                        slots.Add(slot);
                    }
                }
            }
            
            return new Diary()
            {
                Slots = slots
            };
        }

        public static void BookViewing(int customerId, int advertId, Slot slot, bool hasAccompanied)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DiaryDatbse.ConnectionString"].ConnectionString;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                var command = new SqlCommand("sp_updateDiary", sqlConnection) { CommandType = CommandType.StoredProcedure };
                var customerIdIn = command.Parameters.Add("@customerId", SqlDbType.Int);
                customerIdIn.Direction = ParameterDirection.Input;
                customerIdIn.Value = customerId;
                
                var advertIdIn = command.Parameters.Add("@advertId", SqlDbType.Int);
                advertIdIn.Direction = ParameterDirection.Input;
                advertIdIn.Value = advertId;
                
                var slotIn = command.Parameters.Add("@slot", SqlDbType.DateTime);
                slotIn.Direction = ParameterDirection.Input;
                slotIn.Value = slot.StartTime;
                
                var hasAccompaniedIn = command.Parameters.Add("@hasAccompanied", SqlDbType.Bit);
                hasAccompaniedIn.Direction = ParameterDirection.Input;
                hasAccompaniedIn.Value = hasAccompaniedIn;

                command.ExecuteReader();
            }
        }
    }
}