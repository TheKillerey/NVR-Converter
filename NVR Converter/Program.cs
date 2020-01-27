using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantome.Libraries.League;
using Fantome.Libraries.League.Converters;
using Fantome.Libraries.League.IO.AiMesh;
using Fantome.Libraries.League.IO.BIN;
using Fantome.Libraries.League.IO.FX;
using Fantome.Libraries.League.IO.INI;
using Fantome.Libraries.League.IO.Inibin;
using Fantome.Libraries.League.IO.LightDat;
using Fantome.Libraries.League.IO.LightEnvironment;
using Fantome.Libraries.League.IO.LightGrid;
using Fantome.Libraries.League.IO.MapObjects;
using Fantome.Libraries.League.IO.MapParticles;
using Fantome.Libraries.League.IO.MaterialLibrary;
using Fantome.Libraries.League.IO.NVR;
using Fantome.Libraries.League.IO.OBJ;
using Fantome.Libraries.League.IO.SCB;
using Fantome.Libraries.League.IO.SCO;
using Fantome.Libraries.League.IO.SimpleSkin;
using Fantome.Libraries.League.IO.WAD;
using Fantome.Libraries.League.IO.WorldGeometry;
using System.Net;
using System.Threading;

namespace NVR_Converter
{
    class Program
    {
        //Starting the converter
        static void Main(string[] args)
        {
            NVRConvertion();
        }




            static void NVRConvertion()
        {

            string root = @"WGEO";
            //When the Directory don't exist create it! 
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            
            //Checking the wgeo file if exist. If not download.

            if (File.Exists("room.wgeo"))
            {
                Console.WriteLine("File room.wgeo found");
            }
            else
            {
                Console.WriteLine("room.wgeo does not exist and will be downloaded now.");

                WebClient webClient = new WebClient();
                Console.WriteLine("Downloading now");
                webClient.DownloadFile("https://uce213b75ebc39ebd44e366ef7ac.dl.dropboxusercontent.com/cd/0/get/Aw9eirvQFzt_9NL4WBS04jiEjRaXMCO4lU87jdwcHEmoi6lOva-fiY62WlfBO0hOwNjbYrMj16-oyynK5CpxQEQm-lutnygQs8GHMo5YgRJMISRuObmm0IyhyJ4M-XP2ofI/file?dl=1#", @"room.wgeo");
                Console.WriteLine("Finished downloading :)");
            }
            if (File.Exists("room.nvr"))
            {
                Console.WriteLine("File room.nvr found! Convert starts");
            }
            else
            {
                Console.WriteLine("File room.nvr not found. PLS check if room.nvr is right to the exe");
                Thread.Sleep(5000);
                System.Environment.Exit(1);
            }



                //Convert the Simple Enviroment to World Geometry
                NVRFile nvr = new NVRFile("room.nvr");
            WGEOConverter.ConvertNVR(nvr, new WGEOFile("room.wgeo").BucketGeometry).Write("newnvr.wgeo");

            //----------------------------------------------------------------------------------------------//
            //Convert the World Geometry to Object files
            WGEOFile wgeo = new WGEOFile("newnvr.wgeo");

            int index = 0;

            foreach (OBJFile obj in OBJConverter.ConvertWGEOModels(wgeo))
            {

                obj.Write(string.Format("wgeo//{1}_{0}.obj", index, wgeo.Models[index].Material));
                
                //Material creation missing :( 


               







                index++;
            }




            //Delete created WGEO file
            string NewNVRFile = "newnvr.wgeo";
            File.Delete(NewNVRFile);
            Console.WriteLine(NewNVRFile, " deleted.");
            Console.WriteLine("Done");
        }

    }
}