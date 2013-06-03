using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PocketStatsHandler.Contracts;
using PocketStatsHandler.Model;

namespace PocketStatsHandler.Utilities
{
    public class PocketArticleJsonConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      return true;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      var list = new List<IPocketArticle>();
      var pocketArticle = new PocketArticle();
      while (reader.Read())
      {
        if (reader.Value != null)
        {
          string[] strArray = reader.Path.Split('.');

          switch (strArray[strArray.Count() - 1])
          {
            case "item_id":
              pocketArticle.ItemId = reader.ReadAsString();
              break;
            case "resolved_id":
              pocketArticle.ResolvedId = reader.ReadAsString();
              break;
            case "given_url":
              pocketArticle.GivenUrl = reader.ReadAsString();
              break;
            case "given_title":
              pocketArticle.GivenTitle = reader.ReadAsString();
              break;
            case "favorite":
              pocketArticle.Favorite = reader.ReadAsString();
              break;
            case "status":
              pocketArticle.Status = reader.ReadAsString();
              break;
            case "time_added":
              pocketArticle.TimeAdded = reader.ReadAsString();
              break;
            case "time_updated":
              pocketArticle.TimeUpdated = reader.ReadAsString();
              break;
            case "time_read":
              pocketArticle.TimeRead = reader.ReadAsString();
              break;
            case "time_favorited":
              pocketArticle.TimeFavorited = reader.ReadAsString();
              break;
            case "sort_id":
              pocketArticle.SortId = reader.ReadAsInt32().GetValueOrDefault();
              break;
            case "resolved_title":
              pocketArticle.ResolvedTitle = reader.ReadAsString();
              break;
            case "resolved_url":
              pocketArticle.ResolvedUrl = reader.ReadAsString();
              break;
            case "excerpt":
              pocketArticle.Excerpt = reader.ReadAsString();
              break;
            case "is_article":
              pocketArticle.IsArticle = reader.ReadAsString();
              break;
            case "is_index":
              pocketArticle.IsIndex = reader.ReadAsString();
              break;
            case "has_video":
              pocketArticle.HasVideo = reader.ReadAsString();
              break;
            case "has_image":
              pocketArticle.HasImage = reader.ReadAsString();
              break;
            case "word_count":
              pocketArticle.WordCount = reader.ReadAsString();
              break;
          }
        }
        else
        {
          if (!string.IsNullOrEmpty(pocketArticle.ItemId))
            list.Add(pocketArticle);
          pocketArticle = new PocketArticle();
        }
      }
      return list;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }
  }
}
