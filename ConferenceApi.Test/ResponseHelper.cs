using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConferenceApi.Models;

namespace ConferenceApi.Test
{
    public class ResponseHelper
    {
        public static ResponseTopic BuildResponseTopic()
        {
            return new ResponseTopic()
            {
                Collection = BuildCollection()
            };
        }

        public static ResponseSession BuildResponseSession()
        {
            return new ResponseSession()
            {
                Collection = BuildCollection()
            };
        }

        private static Collection BuildCollection()
        {
            return new Collection()
            {
                Items = new[]
                {
                    new Item
                    {
                        Data = new[]
                        {
                            new Datum
                            {
                                Name = Name.Title,
                                Value = "test"
                            }
                        }
                    },
                }
            };
        }
    }
}
