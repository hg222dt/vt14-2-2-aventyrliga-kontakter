using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace contacts.Model.DAL
{
    public class ContactDAL
    {
        #region Fält
        /// <summary>
        /// Sträng med information som används för att ansluta till "SQL-server"-databasen.
        /// (Ett statiskt fält tillhör klassen och delas av alla instanser av klassen).
        /// </summary>
        private static readonly string _connectionString;

        #endregion

        #region Konstruktorer

        /// <summary>
        /// Initierar statisk data. (Konstruktorn anropas automatiskt innan första instansen skapas
        /// eller innan någon statisk medlem används.)
        /// </summary>
        static ContactDAL()
        {
            _connectionString = WebConfigurationManager.ConnectionStrings["contactConnectionString"].ConnectionString;
        }
        #endregion

        #region Privata hjälpmetoder
        /// <summary>
        /// Skapar och initierar ett nytt anslutningsobjekt
        /// </summary>
        /// <returns>Referens till ett nytt SqlConnection-objekt</returns>

        private static SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
        #endregion

        public IEnumerable<Contact> GetContacts()
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["contactConnectionString"].ConnectionString;
        
            using (var conn = new SqlConnection(connectionString))
            {
                var contacts = new List<Contact>(100);

                var cmd = new SqlCommand("Person.uspGetContacts", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();

                using(var reader = cmd.ExecuteReader())
                {
                    var contactIdIndex = reader.GetOrdinal("ContactID");
                    var firstNameIndex = reader.GetOrdinal("FirstName");
                    var lastNameIndex = reader.GetOrdinal("LastName");
                    var emailAddressIndex = reader.GetOrdinal("EmailAddress");


                    while (reader.Read())
                    {
                        contacts.Add(new Contact
                        {
                            ContactId = reader.GetInt32(contactIdIndex),
                            FirstName = reader.GetString(firstNameIndex),
                            LastName = reader.GetString(lastNameIndex),
                            Email= reader.GetString(emailAddressIndex)
                        });
                    }
                }
                
                contacts.TrimExcess();

                return contacts;

            }

        }

        public IEnumerable<Contact> GetContactsPageWise(int maximumRows, int startRowIndex, out int totalRowCount)
        {
            using (var conn = CreateConnection())
            {
                var contacts = new List<Contact>(100);

                var cmd = new SqlCommand("Person.uspGetContactsPageWise", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@PageIndex", SqlDbType.Int, 4).Value = startRowIndex / maximumRows + 1;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int, 4).Value = maximumRows;
                cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                conn.Open();

                cmd.ExecuteNonQuery();

                totalRowCount = (int)cmd.Parameters["@RecordCount"].Value;


                using (var reader = cmd.ExecuteReader())
                {
                    var contactIdIndex = reader.GetOrdinal("ContactID");
                    var firstNameIndex = reader.GetOrdinal("FirstName");
                    var lastNameIndex = reader.GetOrdinal("LastName");
                    var emailAddressIndex = reader.GetOrdinal("EmailAddress");


                    while (reader.Read())
                    {
                        contacts.Add(new Contact
                        {
                            ContactId = reader.GetInt32(contactIdIndex),
                            FirstName = reader.GetString(firstNameIndex),
                            LastName = reader.GetString(lastNameIndex),
                            Email = reader.GetString(emailAddressIndex)
                        });
                    }
                }

                contacts.TrimExcess();

                return contacts;

            }

        }

        public Contact GetContactById(int ContactID)
        {
            using (var conn = CreateConnection())
            {
                try
                {
                    string connectionString = WebConfigurationManager.ConnectionStrings["contactConnectionString"].ConnectionString;

                    var cmd = new SqlCommand("Person.uspGetContact", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ContactID", SqlDbType.Int, 4).Value = ContactID;

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        var contactIdIndex = reader.GetOrdinal("ContactID");
                        var firstNameIndex = reader.GetOrdinal("FirstName");
                        var lastNameIndex = reader.GetOrdinal("LastName");
                        var emailAddressIndex = reader.GetOrdinal("EmailAddress");

                        if (reader.Read())
                        {
                            return new Contact
                            {
                                ContactId = reader.GetInt32(contactIdIndex),
                                FirstName = reader.GetString(firstNameIndex),
                                LastName = reader.GetString(lastNameIndex),
                                Email = reader.GetString(emailAddressIndex)
                            };
                        }
                    }

                    return null;
                }
                catch
                {
                    throw new ApplicationException("An error occured while getting contacts from database.");
                }
            }
        }

        public void InsertContact(Contact Contact)
        {
            using (var conn = CreateConnection())
            {
                try
                {
                    string connectionString = WebConfigurationManager.ConnectionStrings["contactConnectionString"].ConnectionString;

                    var cmd = new SqlCommand("Person.uspAddContact", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = Contact.FirstName;
                    cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = Contact.LastName;
                    cmd.Parameters.Add("@EmailAddress", SqlDbType.NVarChar, 50).Value = Contact.Email;
                    cmd.Parameters.Add("@ContactID", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    conn.Open();

                    cmd.ExecuteNonQuery();

                    Contact.ContactId = (int)cmd.Parameters["@ContactID"].Value;
                }
                catch
                {
                    throw new ApplicationException("An error occured while adding contacts from database.");
                }
            }
        }

        public void UpdateContact(Contact Contact)
        {
            //Skapar och initierar anslutningobjekt
            using(SqlConnection conn = CreateConnection ())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Person.uspUpdateContact", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = Contact.FirstName;
                    cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = Contact.LastName;
                    cmd.Parameters.Add("@EmailAddress", SqlDbType.NVarChar, 50).Value = Contact.Email;
                    cmd.Parameters.Add("@ContactID", SqlDbType.Int, 4).Value = Contact.ContactId;

                    conn.Open();

                    cmd.ExecuteNonQuery();

                }
                catch
                {
                    throw new ApplicationException("Problem occured while updating contact.");
                }
            }
        }

        public void DeleteContact(int ContactID)
        {
            //Skapar och initierar anslutningobjekt
            using (SqlConnection conn = CreateConnection())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Person.uspRemoveContact", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ContactID", SqlDbType.Int, 4).Value = ContactID;

                    conn.Open();

                    cmd.ExecuteNonQuery();

                }
                catch
                {
                    throw new ApplicationException("Problem occured while updating contact.");
                }
            }
        }
    }
}