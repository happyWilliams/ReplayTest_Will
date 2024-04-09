using System.Collections.Generic;
using DataUtilities = DataAnalysis.Utilities;

namespace DataAnalysis
{
    public class PropertyManager
    {
        private static PropertyManager instance;

        private PropertyManager()
        {
        }

        public static PropertyManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PropertyManager();
                }

                return instance;
            }
        }

        private List<FrameDatum> frameData;

        public void InitializeData()
        {
            frameData = DataUtilities.ReadIdfFileByLines<FrameDatum>("Assets/DataAssets/Applicant-test.idf");
            
        }
    }
}