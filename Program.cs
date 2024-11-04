using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

internal class Program
{
    private static int _sendTimeDelimiterSeconds = 1;

    private static async Task Main(string[] args)
    {
        using Socket socket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Raw,
            ProtocolType.Icmp
        );

        int transmittedPackets = 4;

        if (args.FirstOrDefault() is null)
            throw new InvalidDataException("destination address is required");
        if (args.Count() > 1 && args[1] is not null)
            int.TryParse(args[1], out transmittedPackets);

        var host = args[0];

        byte[] receivedBytes = new byte[64];

        int receivedPackets = await SendPacketsAsync(
            transmittedPackets,
            host,
            socket,
            receivedBytes
        );

        CountAndWriteStatistics(transmittedPackets, receivedPackets);

        socket.Close();
    }

    private static void CountAndWriteStatistics(int transmittedPackets, int receivedPackets)
    {
        int percentage = transmittedPackets % receivedPackets;

        Console.WriteLine(
            "{0} packets transmitted, {1} packets received, {2}% loss",
            transmittedPackets,
            receivedPackets,
            percentage
        );
    }

    private static async Task<int> SendPacketsAsync(
        int count,
        string host,
        Socket socket,
        byte[] bytes
    )
    {
        int receivedPackets = 0;
        for (int i = 0; i < count; i++)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var dnsHostEntry = await Dns.GetHostEntryAsync(host);
                IPEndPoint endPoint = new IPEndPoint(dnsHostEntry.AddressList.First(), 0);
                int result = await socket.SendToAsync(bytes, endPoint);
                stopwatch.Stop();
                Console.WriteLine(
                    "destination address: {0}; destination host: {1}, bytes: {2}, result: {3}; time: {4} ms; time-to-live: {5}; try: {6}",
                    host,
                    endPoint.Address,
                    bytes.Length,
                    result,
                    stopwatch.Elapsed.TotalMilliseconds,
                    socket.Ttl,
                    i + 1
                );

                if (result == bytes.Length)
                    receivedPackets++;
            }
            catch
            {
                Console.WriteLine(
                    "destination address: {0}; status: unable to ping; try: {1}",
                    host,
                    i
                );
            }
            finally
            {
                Thread.Sleep(_sendTimeDelimiterSeconds * 1000);
                stopwatch.Reset();
            }
        }

        return receivedPackets;
    }
}
