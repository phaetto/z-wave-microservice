namespace ZHostService
{
    using System;
    using System.Threading;
    using ZHostService.Actions;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var zWaveContext = new ZWaveContext())
                {
                    zWaveContext.OnStart();

                    GetValue(61, zWaveContext);
                    GetValue(62, zWaveContext);

                    // zWaveContext.Do(new SetConfigurationValue(2, 62, 8));

                    Console.WriteLine();
                    Console.WriteLine("1. Is on: {0}", zWaveContext.DoRemotable(new IsOnState(2)));
                    zWaveContext.Do(new SwitchOn(2));

                    Thread.Sleep(5000);

                    Console.WriteLine("2. Is on: {0}", zWaveContext.DoRemotable(new IsOnState(2)));
                    zWaveContext.Do(new SwitchOff(2));

                    Console.WriteLine();
                    Console.WriteLine("Finished.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            Console.ReadLine();
        }

        private static void GetValue(byte configId, ZWaveContext zWaveContext)
        {
            Console.WriteLine();
            Console.WriteLine("{0} = {1}", configId, zWaveContext.DoRemotable(new GetConfigurationValue(2, configId)));
            Console.WriteLine();
        }
    }
}
