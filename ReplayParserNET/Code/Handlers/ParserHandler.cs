using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReplayParserNET.Warcraft3;
using System.Threading;
using System.IO;

namespace ReplayParserNET.Handlers
{
    static class ParserHandler
    {
        //OTHER FUNCTIONALITY:
        //incorporate the parser as a thread so large databases of replays can be parsed quickly
        //need to look into graphing libraries for the charts
        //apm chart
        //actions chart
        //build chart, etc
        //the actions need to be documented more clearly, so an "action list" can be created
        //see w3gmaster implementation on this
        //look in to windows system calls, see if they can be done without a command prompt
        //this will make the batch renaming a lot smoother
        //this will also make launching warcraft 3 nicer too

        public static HashSet<String> LastFailedReplays { get; set; }

        static ParserHandler()
        {
            LastFailedReplays = new HashSet<String>();
        }

        public static List<Warcraft3Replay> ParseWarcraft3Replays(List<String> filePaths)
        {
            int numberOfTasks = filePaths.Count;
            List<Warcraft3Replay> replays = new List<Warcraft3Replay>(numberOfTasks);
            LastFailedReplays.Clear();

            if (numberOfTasks == 0)
                return replays;

            //scope, so the garbage collection handles the unused memory
            {
                Warcraft3ReplayParser[] tasks = new Warcraft3ReplayParser[numberOfTasks];
                ManualResetEvent signal = new ManualResetEvent(false);

                for (int i = 0; i < filePaths.Count; i++)
                {
                    //this reference is needed so the task can be referenced in a threadsafe manner
                    Warcraft3ReplayParser task = new Warcraft3ReplayParser(filePaths[i]);
                    tasks[i] = task;

                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        try
                        {
                            task.ParseReplay();
                        }

                        //TODO: need some sort of handling, perhaps log the error??
                        catch (Exception) { }

                        //when the task is complete, we decrement the total number of tasks left
                        finally
                        {
                            if (Interlocked.Decrement(ref numberOfTasks) == 0)
                                signal.Set();
                        }
                    });
                }

                //when all tasks are complete, the resetevent is set and we can proceed
                signal.WaitOne();

                for (int i = 0; i < filePaths.Count; i++)
                {
                    if (tasks[i].Replay != null)
                    {
                        replays.Add(tasks[i].Replay);
                    }
                    else
                    {
                        LastFailedReplays.Add(tasks[i].FilePath);
                    }
                }

//#if (DEBUG)
//                TextWriter output = new StreamWriter("..\\..\\Resources\\errors.txt");

//                //reading through all of the replays
//                for (int i = 0; i < filePaths.Length; i++)
//                {
//                    if (tasks[i].Replay != null)
//                       ;//output.WriteLine("Success: " + filePaths[i]);
//                    else
//                        output.WriteLine("Error: " + filePaths[i]);
//                }

//                output.Close();
//#endif
            } //parsers go out of scope, garbage collected

            return replays;
        }

        public static Warcraft3Replay ParseWarcraft3Replay(String filePath)
        {
            Warcraft3ReplayParser parser = new Warcraft3ReplayParser(filePath);
            LastFailedReplays.Clear();
            parser.ParseReplay();

            if (parser.Replay == null)
                LastFailedReplays.Add(parser.FilePath);

            return parser.Replay;
        }
    }
}
