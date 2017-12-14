


////Modified from http://xnaessentials.com/tutorials/highscores.aspx
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Xml.Linq;
namespace ShootShapesUp
{
    [Serializable]
    public struct HighScoreData
    {
        public string[] PlayerName;
        public int[] Score;
        public int Count;

        public HighScoreData(int count)
        {
            PlayerName = new string[count];
            Score = new int[count];
            Count = count;
        }
    }
    public class Leaderboard {

        public readonly string HighScoresFilename = "highscores.xml";
        public static  String Location = "D:/Year 3/CS3005 - Digital Media and Games/Visual Studio 2015/Projects/ShootShapesUp/ShootShapesUp";
        public static string[] names = new string[10];
        public static int[] scores = new int[10];
        public void Initialize()
        {
            // Get the path of the save game
            string fullpath = Path.Combine(Location, HighScoresFilename);

            // Check to see if the save exists
            if (!File.Exists(fullpath))
            {
                //If the file doesn't exist, make a fake one...
                // Create the data to save
                HighScoreData data = new HighScoreData(10);
                data.PlayerName[0] = "Neil";
                //data.Level[0] = 10;
                data.Score[0] = 1;

                data.PlayerName[1] = "Shawn";
                //data.Level[1] = 10;
                data.Score[1] = 2;

                data.PlayerName[2] = "Mark";
                //data.Level[2] = 9;
                data.Score[2] = 3;

                data.PlayerName[3] = "Cindy";
                //data.Level[3] = 7;
                data.Score[3] = 4;

                data.PlayerName[4] = "Sam";
                //data.Level[4] = 1;
                data.Score[4] = 5;

                SaveHighScores(data, HighScoresFilename);
            }


        }


        public static void SaveHighScores(HighScoreData data, String fileName)
    {
        // Get the path of the save game
        string fullpath = Path.Combine(Location, fileName);

        // Open the file, creating it if necessary
        FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate);
        try
        {
            // Convert the object to XML data and put it in the stream
            XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
            serializer.Serialize(stream, data);
        }
        finally
        {
            // Close the file
            stream.Close();
        }
    }
    public static HighScoreData LoadHighScores(String fileName)
    {
        HighScoreData data;

        // Get the path of the save game
        string fullpath = Path.Combine(Location, fileName);

        // Open the file
        FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate,
        FileAccess.Read);
        try
        {
                // Read the data from the file
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                Console.WriteLine(fullpath);
                //String a =PrettyXml(fullpath);
                //Console.WriteLine(a);
                data = (HighScoreData)serializer.Deserialize(stream);

            }
            finally
        {
            // Close the file
            stream.Close();
        }
            
            return (data);
    }
        public static void PrettyXml(string xml)
        {


            XElement xelement = XElement.Load(xml);
             IEnumerable<XElement> name = xelement.Descendants("PlayerName");
            IEnumerable<XElement> score = xelement.Descendants("Score");
            // Read the entire XML
            for (int i = 0; i<10 ;i++)
            {
                Console.WriteLine("name: " + score.ToString());

            }

            //foreach (var employee in score.Nodes())
            //{

            //    Console.WriteLine("name: " + score.Nodes());
            //    //Console.WriteLine("Name: " + employee.Attribute("<PlayerName>"));
            //}
           
        }

        public void OrderSave()
    {
        // Create the data to save
        HighScoreData data = LoadHighScores(HighScoresFilename);

        int scoreIndex = -1;
        for (int i = 0; i < data.Count; i++)
        {
            if (Score.score > data.Score[i])
            {
                scoreIndex = i;
                break;
            }
        }

        if (scoreIndex > -1)
        {
            //New high score found ... do swaps
            for (int i = data.Count - 1; i > scoreIndex; i--)
            {
                data.PlayerName[i] = data.PlayerName[i - 1];
                data.Score[i] = data.Score[i - 1];
                //data.Level[i] = data.Level[i - 1];
            }

            data.PlayerName[scoreIndex] = "Frankie"; //Retrieve User Name Here
            data.Score[scoreIndex] = Score.score;

            SaveHighScores(data, HighScoresFilename);
        }
    }

}
    }






