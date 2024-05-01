using GameData;
using Newtonsoft.Json;
using UnityEngine;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        public void SaveStampData(StampData currentStampData)
        {
            // Convert the ToolData object to JSON
            var serializedData = JsonConvert.SerializeObject(currentStampData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            // Save the serialized data to PlayerPrefs
            PlayerPrefs.SetString("SavedStampData", serializedData);
            PlayerPrefs.Save();
        }
        public void SaveDrawData(DrawData currentDrawData)
        {
            // Convert the ToolData object to JSON
            var serializedData = JsonConvert.SerializeObject(currentDrawData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            // Save the serialized data to PlayerPrefs
            PlayerPrefs.SetString("SavedDrawData", serializedData);
            PlayerPrefs.Save();
        }
        public void SaveEraserData(EraserData currentEraserData)
        {
            // Convert the ToolData object to JSON
            var serializedData = JsonConvert.SerializeObject(currentEraserData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            // Save the serialized data to PlayerPrefs
            PlayerPrefs.SetString("SavedEraserData", serializedData);
            PlayerPrefs.Save();
        }
        public void SaveBucketData(BucketData currentBucketData)
        {
            // Convert the ToolData object to JSON
            var serializedData =JsonUtility.ToJson(currentBucketData);
            // Save the serialized data to PlayerPrefs
            PlayerPrefs.SetString("SavedBucketData", serializedData);
            PlayerPrefs.Save();
        }
        public void SaveSplashData(SplashData currentStampData)
        {
            // Convert the SplashData object to JSON
            var serializedData = JsonConvert.SerializeObject(currentStampData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            // Save the serialized data to PlayerPrefs
            PlayerPrefs.SetString("SavedSplashData", serializedData);
            PlayerPrefs.Save();
        }
        public SplashData LoadSplashData()
        {
            // Retrieve the saved serialized data from PlayerPrefs
            var serializedData = PlayerPrefs.GetString("SavedSplashData");

            // Convert the serialized data back to SplashData object
            var savedSplashData = JsonConvert.DeserializeObject<SplashData>(serializedData);

            return savedSplashData;
        }
        public BucketData LoadBucketData()
        {
            // Retrieve the saved serialized data from PlayerPrefs
            var serializedData = PlayerPrefs.GetString("SavedBucketData");

            // Convert the serialized data back to BucketData object
            var savedBucketData = JsonUtility.FromJson<BucketData>(serializedData);

            return savedBucketData;
        }

        public StampData LoadStampData()
        {
            // Retrieve the saved serialized data from PlayerPrefs
            var serializedData = PlayerPrefs.GetString("SavedStampData");

            // Convert the serialized data back to StampData object
            var savedStampData = JsonConvert.DeserializeObject<StampData>(serializedData);

            return savedStampData;
        }
        public DrawData LoadDrawData()
        {
            // Retrieve the saved serialized data from PlayerPrefs
            var serializedData = PlayerPrefs.GetString("SavedDrawData");

            // Convert the serialized data back to DrawData object
            var savedDrawData = JsonConvert.DeserializeObject<DrawData>(serializedData);

            return savedDrawData;
        }
        public EraserData LoadEraserData()
        {
            // Retrieve the saved serialized data from PlayerPrefs
            var serializedData = PlayerPrefs.GetString("SavedEraserData");

            // Convert the serialized data back to EraserData object
            var savedEraserData = JsonConvert.DeserializeObject<EraserData>(serializedData);

            return savedEraserData;
        }
    }
}