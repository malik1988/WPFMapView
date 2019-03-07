using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.IO;
namespace MAP.MapViewObjects
{
  public  class MapData
  {        
      /// <summary>
      /// 地图名称
      /// </summary>
      public string title;
      public List<MapLine> lines;

      public static T Load<T>(string path)
      {

          try
          {
              using (StreamReader sr = new StreamReader(path))
              {
                  return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
              }
          }
          catch (Exception e)
          {
              return default(T);
          }
      }

    }

  public class MapLine
  {
      /// <summary>
      /// 起点名
      /// </summary>
      public string start;
      /// <summary>
      /// 终点名
      /// </summary>
      public string end;
      /// <summary>
      /// 起点X
      /// </summary>
      public int startX;
      /// <summary>
      /// 起点Y
      /// </summary>
      public int startY;
      /// <summary>
      /// 终点X
      /// </summary>
      public int endX;
      /// <summary>
      /// 终点Y
      /// </summary>
      public int endY;
      /// <summary>
      /// 线类型：1-直线，2-逆时针曲线，3-顺时针曲线
      /// </summary>
      public string type;
      /// <summary>
      /// 是否双向线
      /// </summary>
      public bool isTwoWay = false;
  }

}
