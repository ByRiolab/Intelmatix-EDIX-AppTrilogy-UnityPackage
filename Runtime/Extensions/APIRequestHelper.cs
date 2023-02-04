using System.Threading.Tasks;
using Intelmatix.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace Intelmatix.Networking
{
    /// <summary>
    /// <description>
    /// This class is a static class that can be used to make requests to the API
    /// </description><br/>
    /// @author:  Anthony Shinomiya M.
    /// @date:  2022-01-21
    /// </summary>
    public static class APIRequestHelper
    {
        private static APIConfig apiConfig;

        public static void Init(APIConfig apiConfig)
        {
            APIRequestHelper.apiConfig = apiConfig;
        }

        public static async Task<T> Get<T>(string path, bool useAPIConfig = true)
        {
            string url = (useAPIConfig && apiConfig != null ? apiConfig.API : "") + path;

            using var www = UnityWebRequest.Get(url);

            www.SetRequestHeader("Content-Type", "application/json");
            if (useAPIConfig && apiConfig != null)
                www.SetRequestHeader("Authorization", apiConfig.AuthorizationKey);

            var operation = www.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Failed: {www.error}");

            try
            {
                var result = JsonUtility.FromJson<T>(www.downloadHandler.text);
                Debug.Log($"Success request: {url}");
                return result;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"{nameof(Get)} failled: {e.Message}");
                return default;
            }
        }


        #region Sprites
        // public async Task<Sprite> GetSprite(string url)
        // {
        //     var texture = await GetTexture(url);
        //     Sprite sprite = Sprite.Create
        //     (
        //         texture, 
        //         new Rect(0,0,texture.width,texture.height),
        //         Vector2.zero
        //     );
        //     return sprite;
        // }

        // public async Task<Texture2D> GetTexture(string url)
        // {
        //     using var www = UnityWebRequestTexture.GetTexture(url);
        //     www.SetRequestHeader("Content-Type", "application/json");
        //     var operation = www.SendWebRequest();

        //     while(!operation.isDone)
        //         await Task.Yield();

        //     if (www.result != UnityWebRequest.Result.Success)
        //         Debug.LogError($"Failed: {www.error}");

        //     try
        //     {
        //         var texture = DownloadHandlerTexture.GetContent(www);
        //         // www.Dispose();
        //         return texture;
        //     }
        //     catch(System.Exception e)
        //     {
        //         Debug.LogError($"{nameof(GetTexture)} failled: {e.Message}");
        //         return default;
        //     }
        // }
        #endregion

        #region Model
        // public async Task<AssetBundle> GetModel(string url)
        // {
        //     using var www = UnityWebRequestAssetBundle.GetAssetBundle(url);
        //     www.SetRequestHeader("Content-Type", "application/json");
        //     var operation = www.SendWebRequest();
        //     while (!operation.isDone)
        //         await Task.Yield();

        //     if (www.result != UnityWebRequest.Result.Success)
        //         Debug.LogError($"Failed: {www.error} {url}");

        //     try
        //     {
        //         var bundle = DownloadHandlerAssetBundle.GetContent(www);
        //         return bundle;
        //     }
        //     catch (System.Exception e)
        //     {
        //         Debug.LogError($"{nameof(GetModel)} failled: {e.Message}");
        //         return null;
        //     }
        // }
        #endregion

        #region Sound
        // public async Task<AudioClip> GetAudioClip(string url)
        // {
        //     using var www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);
        //     www.SetRequestHeader("Content-Type", "application/json");
        //     var operation = www.SendWebRequest();
        //     while (!operation.isDone)
        //         await Task.Yield();

        //     if (www.result != UnityWebRequest.Result.Success)
        //         Debug.LogError($"Failed: {www.error} {url}");

        //     try
        //     {
        //         var clip = DownloadHandlerAudioClip.GetContent(www);
        //         return clip;
        //     }
        //     catch (System.Exception e)
        //     {
        //         Debug.LogError($"{nameof(GetAudioClip)} failled: {e.Message}");
        //         return null;
        //     }
        // }
        #endregion

        #region method: put
        // public async void Put<T>(string url, T transform)
        // {

        //     using var www = UnityWebRequest.Put(url, JsonUtility.ToJson(transform));

        //     www.SetRequestHeader("Content-Type", "application/json");

        //     var operation = www.SendWebRequest();

        //     while (!operation.isDone)
        //         await Task.Yield();

        //     if (www.result == UnityWebRequest.Result.Success)
        //         Debug.Log($"Success: {www.downloadHandler.text}");
        //     else
        //         Debug.LogError($"Failed: {www.error}");

        // }
        #endregion

        #region  method: delete
        // public async void Delete(string url)
        // {
        //     using var www = UnityWebRequest.Delete(url);

        //     www.SetRequestHeader("Content-Type", "application/json");

        //     var operation = www.SendWebRequest();

        //     while (!operation.isDone)
        //         await Task.Yield();


        //     if (www.result == UnityWebRequest.Result.Success)
        //         Debug.Log($"Success Delete");
        //     else
        //         Debug.LogError($"Failed: {www.error}");

        // }
        #endregion
    }
}