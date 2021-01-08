/*
 * Created by SharpDevelop.
 * User: 26468
 * Date: 2021/1/8
 * Time: 12:16
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using RGSnowflake;

namespace Test
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			Console.WriteLine("输入数据中心编号");
			long dcid=Convert.ToInt64(Console.ReadLine()); 
			Console.WriteLine("输入机器编号");
			long mcid=Convert.ToInt64(Console.ReadLine());
			SnowflakeGenerator g=new SnowflakeGenerator(mcid,dcid,DateTime.Now);
			Console.WriteLine("生成的id：");
			Console.WriteLine(g.nextID());
			PrintBit(g.nextID());
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		public static void PrintBit(long l){
			for(int i=1;i<=63;i++){
				Console.Write(l&1);
				l>>=1;
			}
			if(l>=0){
				Console.Write(0);
			}else{
				Console.Write(1);
			}
			Console.WriteLine("");
		}
	}
}