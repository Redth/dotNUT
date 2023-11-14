// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Hello, World!");
CancellationTokenSource cts = new CancellationTokenSource();

var tcp = new TcpClient();

await tcp.ConnectAsync("192.168.2.3", 3493);

var ns = tcp.GetStream();

_ = Task.Run(async () =>
{
	using var streamReader = new StreamReader(ns);

	var buffer = new Memory<char>(new char[1024]);

	while (!cts.IsCancellationRequested)
	{
		var read = await streamReader.ReadAsync(buffer, cts.Token);

		if (read > 0)
		{
			var text = buffer.Slice(0, read).ToString();
			Console.WriteLine($"RX: {text}");

			var c = new Salaros.Configuration.ConfigParser(text);

		}
		else
		{
			break;
		}




	
	}

});


using var streamWriter = new StreamWriter(ns);

//await streamWriter.WriteLineAsync("ATTACH ups@192.168.2.3:3493");
await streamWriter.WriteLineAsync("HELP");
//await streamWriter.WriteLineAsync("USERNAME upsmon");
//await streamWriter.WriteLineAsync("PASSWD secret");
//await streamWriter.WriteLineAsync("LOGIN upsmon");

Console.ReadLine();
cts.Cancel();


