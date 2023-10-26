/////
/// Author: 
/////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public interface IEventHandler
    {
        public String handleFile();
        public String handleChatMessage();
        public String handleAnalyserResult();
    }
}
