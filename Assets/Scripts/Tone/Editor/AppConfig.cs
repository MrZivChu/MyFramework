using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tone
{
    public static class AppConfig
    {
        public const string APP_NAME = "M2";
        public const long CLICK_TIMESPAN = 500;  //两次点击的间隔，以毫秒为单位

        //Log
        public const int MAX_LOG_FILE = 3;      //日志文件的最大数量，每次运行产生一个日志文件，为避免日志文件太多，需要限定数量

        //Download
        public const int MAX_DOWNLOAD_TASKS = 3;    //最大同时下载数量
        public const int MAX_TRY_TIMES = 3;         //最大重试次数
        public const float MAX_ZOMBIE_SECONDS = 20.0f;   //最大僵尸时间
    }
}
