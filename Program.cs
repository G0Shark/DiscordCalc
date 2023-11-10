using System.Text.RegularExpressions;
using Discord;
using Discord.WebSocket;

namespace SHRK_DISOCRDBOT{
	public static class Utils{
		public static bool RegexConst(string regex, string text){
			return new Regex(regex).IsMatch(text);
		}
		public static string[] ForCalc(string text){
			char[] temp = text.ToCharArray();
			string temp2 = "";
			List<string> strings = new List<string>();
			for(int i = 0; i < temp.Length; i++){
				if (temp[i]=='+' || temp[i]=='-' || temp[i]=='x' || temp[i]=='/' || temp[i]=='*'){
					strings.Add(temp2);
					strings.Add(temp[i].ToString());
					temp2 = "";
				}else {
					temp2+=temp[i].ToString();
				}
			}
			strings.Add(temp2);
			return strings.ToArray();
		}
	}
	
	class Program{
		DiscordSocketClient client;
		public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
		private async Task MainAsync(){
			var config = new DiscordSocketConfig() 
			{
    			GatewayIntents = GatewayIntents.All
			};

			client = new DiscordSocketClient(config);
			client.MessageReceived += CommandsHandler;
			client.Log += Log;

			var token = File.ReadAllText(".token.txt") ?? "null"; //Read token

			await client.LoginAsync(TokenType.Bot, token);
			await client.StartAsync();

			Console.ReadLine();
			await client.LogoutAsync();
		}
		private Task CommandsHandler(SocketMessage message){
			if(!message.Author.IsBot){
				if (Utils.RegexConst("[0-9]*[+-x/][0-9]*", message.Content)){
					string[] forcalc = Utils.ForCalc(message.Content);
					switch (forcalc[1]){
						case "+":{
							int temp = int.Parse(forcalc[0]) + int.Parse(forcalc[2]);
							message.Channel.SendMessageAsync(forcalc[0] + " + " + forcalc[2] + " = " + temp);
							break;
						}
						case "-":{
							int temp = int.Parse(forcalc[0]) - int.Parse(forcalc[2]);
							message.Channel.SendMessageAsync(forcalc[0] + " - " + forcalc[2] + " = " + temp);
							break;
						}
						case "*":{
							int temp = int.Parse(forcalc[0]) * int.Parse(forcalc[2]);
							message.Channel.SendMessageAsync(forcalc[0] + " x " + forcalc[2] + " = " + temp);
							break;
						}
						case "x":{
							int temp = int.Parse(forcalc[0]) * int.Parse(forcalc[2]);
							message.Channel.SendMessageAsync(forcalc[0] + " x " + forcalc[2] + " = " + temp);
							break;
						}
						case "/":{
							int temp = int.Parse(forcalc[0]) / int.Parse(forcalc[2]);
							message.Channel.SendMessageAsync(forcalc[0] + " / " + forcalc[2] + " = " + temp);
							break;
						}
					}
				}
			}
			return Task.CompletedTask;
		}
		private Task Log(LogMessage message){
			Console.WriteLine(message.ToString());
			return Task.CompletedTask;
		}
	}
}