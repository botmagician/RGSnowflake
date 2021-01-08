/*
 * Created by SharpDevelop.
 * User: stu01
 * Date: 2021-01-05
 * Time: 21:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace RGSnowflake
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public class SnowflakeGenerator
	{
		//因为二进制里第一个 bit 为如果是 1，那么都是负数，但是我们生成的 id 都是正数，所以第一个 bit 统一都是 0。
		//机器ID  2进制5位  32位减掉1位 31个
		private long workerId;
		//机房ID 2进制5位  32位减掉1位 31个
		private long datacenterId;
		//代表一毫秒内生成的多个id的最新序号  12位 4096 -1 = 4095 个
		private long sequence;
		//设置一个时间初始值    2^41 - 1   差不多可以用69年
		private long twepoch = 1585644268888L;
		//5位的机器id
		private long workerIdBits = 5L;
		//5位的机房id
		private long datacenterIdBits = 5L;
		//每毫秒内产生的id数 2 的 12次方
		private long sequenceBits = 12L;
		// 这个是二进制运算，就是5 bit最多只能有31个数字，也就是说机器id最多只能是32以内
		private long maxWorkerId =31;
		// 这个是一个意思，就是5 bit最多只能有31个数字，机房id最多只能是32以内
		private long maxDatacenterId =31;
		//记录产生时间毫秒数，判断是否是同1毫秒
		private long lastTimestamp = -1L;
		public long getWorkerId(){
			return workerId;
		}
		public long getDatacenterId() {
			return datacenterId;
		}
		public long getTimestamp() {
			return lastTimestamp;
		}
		public long timeNow() {
			return DateTime.Now.Ticks/TimeSpan.TicksPerMillisecond;
		}
		public SnowflakeGenerator(long workerID,long datacenterID,DateTime StartTime){
			if(workerID>maxWorkerId||workerID<0){
				throw new Exception();
			}
			if(datacenterID>maxDatacenterId||datacenterID<0){
				throw new Exception();
			}
			if(StartTime.Ticks>DateTime.Now.Ticks){
				throw new Exception();
			}
			this.datacenterId=workerID;
			this.workerId=workerID;
			this.sequence=0;
			this.twepoch=StartTime.Ticks/TimeSpan.TicksPerMillisecond;
		}
		private long waitForNextTimestamp(){
			long StampNow=this.timeNow();
			while(this.timeNow()<=lastTimestamp){
				StampNow=this.timeNow();
			}
			return StampNow;
		}
		public long nextID(){
			lock(this){
				if(this.timeNow()<lastTimestamp){
					throw new Exception();
				}
				long timestamp=this.timeNow();
				if(lastTimestamp==timestamp){
					if(sequence>=4095){
						lastTimestamp=waitForNextTimestamp();
						sequence=0;
					}
				}else{
					sequence=0;
				}
				sequence++;
				long result=((timestamp-twepoch)<<22)|(datacenterId<<17)|(workerId<<12)|sequence;
				lastTimestamp=timestamp;
				return result;
			}
			
		}
	}
}