using Sabio.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data;
using Sabio.Models.Requests.Addresses;
using Sabio.Models.Domain.Addresses;
using Sabio.Services.Interfaces;

namespace Sabio.Services
{
    public class AddressService : IAddressService
    {
        IDataProvider _data = null;

        public AddressService(IDataProvider data)
        {
            _data = data;
        }




        public void Delete(int id)
        {
            string procName = "[dbo].[Sabio_Addresses_DeleteById]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                
                collection.AddWithValue("@Id", id);

            },
                returnParameters: null);

        }

        public void Update(AddressUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Sabio_Addresses_Update]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection collection)
                {
                    AddCommonParams(model, collection);

                    collection.AddWithValue("@Id", model.Id);

                },
                returnParameters: null);

        }

        public int Add(AddressAddRequest model, int userId)
        {
            int id = 0;


            string procName = "[dbo].[Sabio_Addresses_Insert]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection collection)
                {
                    AddCommonParams(model, collection);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    collection.Add(idOut);


                },
                returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object objectId = returnCollection["@Id"].Value;

                    int.TryParse(objectId.ToString(), out id);

                });

            return id;
        }

        public Address Get(int id)
        {
            string procName = "[dbo].[Sabio_Addresses_SelectById]";

            Address address = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            },
            delegate (IDataReader reader, short set) //single record mapper, will execute one time for every record in the database
            {
                address = MapSingleAddress(reader);
            }
            );

            return address;
        }

        public List<Address> GetRandomAddresses()
        {
            List<Address> addressList = null;
            string procName = "[dbo].[Sabio_Addresses_SelectRandom50]";

            _data.ExecuteCmd(procName, inputParamMapper: null
                , singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    Address anAddress = MapSingleAddress(reader);

                    if (addressList == null)
                    {
                        addressList = new List<Address>();

                    }

                    addressList.Add(anAddress);
                }
            );
            return addressList;
        }



        private static Address MapSingleAddress(IDataReader reader)
        {
            Address anAddress = new Address();

            int startingIdex = 0;

            anAddress.Id = reader.GetSafeInt32(startingIdex++);
            anAddress.LineOne = reader.GetSafeString(startingIdex++);
            anAddress.SuiteNumber = reader.GetSafeInt32(startingIdex++);
            anAddress.City = reader.GetSafeString(startingIdex++);
            anAddress.State = reader.GetSafeString(startingIdex++);
            anAddress.PostalCode = reader.GetSafeString(startingIdex++);
            anAddress.IsActive = reader.GetSafeBool(startingIdex++);
            anAddress.Lat = reader.GetSafeDouble(startingIdex++);
            anAddress.Long = reader.GetSafeDouble(startingIdex++);
            return anAddress;
        }

        private static void AddCommonParams(AddressAddRequest model, SqlParameterCollection collection)
        {
            collection.AddWithValue("@LineOne", model.LineOne);
            collection.AddWithValue("@SuiteNumber", model.SuiteNumber);
            collection.AddWithValue("@City", model.City);
            collection.AddWithValue("@State", model.State);
            collection.AddWithValue("@PostalCode", model.PostalCode);
            collection.AddWithValue("@IsActive", model.IsActive);
            collection.AddWithValue("@Lat", model.Lat);
            collection.AddWithValue("@Long", model.Long);
        }
    }
}
