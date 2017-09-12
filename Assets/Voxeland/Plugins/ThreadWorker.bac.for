using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;


namespace Plugins
{
	public class ThreadWorker 
	{
		public static Thread mainThread = null;




		public class ConditionQueue<T>
		{
			private List<T> list = new List<T>();
			private int count = 0;

			public T Dequeue () 
			{ 
				if (count==0) return default(T);
				T ent=list[0]; 
				list.RemoveAt(0); 
				count--; 
				return ent; 
			}

			public T Dequeue (Func<bool> condition) //dequeues first element with condition met
			{
				for (int i=0; i<count; i++)
					if (condition==null || condition())
					{
						T ent = list[i];
						list.RemoveAt(i);
						count--;
						return ent;
					}
				return default(T);
			}

			public void Enqueue (T ent) { list.Add(ent); count++; }

			public void CheckRemove (T ent)
			{
				if (list.Contains(ent)) {list.Remove(ent); count--;}
			}
		}


		public static int maxThreads = 2; 
		public static int threadsRunning = 0;
		public static List<ThreadWorker> threadsQueue = new List<ThreadWorker>(); //have to remove from queue on restart, so it's actually List

		public static int maxApply = 1; 
		public static List<ThreadWorker> applyQueue = new List<ThreadWorker>();

		public static void Refresh ()
		{
			//checking main thread for IsMainThread function
			if (mainThread==null) mainThread = Thread.CurrentThread;
			
			//starting threads
			for (int i=0; i<threadsQueue.Count; i++)
			{
				//max threads guard
				if (threadsRunning >= maxThreads) break;

				//dequeue first worker if it's condition is met
				ThreadWorker worker = null;
				if (threadsQueue[i].threadCondition==null || threadsQueue[i].threadCondition())
				{
					worker = threadsQueue[i];
					threadsQueue.RemoveAt(i); i--;
				}
				if (worker==null) break; //no suitable threads

				//starting thread
				worker.thread = new Thread(worker.ThreadFn);
				worker.thread.IsBackground = true;
				worker.thread.Start();
				worker.stage = Stage.threadRunning;

				threadsRunning++;
			}

			//apply threads
			int applyNow = 0;
			for (int i=0; i<applyQueue.Count; i++)
			{
				//max apply guard
				if (applyNow >= maxApply) break;

				//dequeue first worker if it's condition is met
				ThreadWorker worker = null;
				if (applyQueue[i].applyCondition==null || applyQueue[i].applyCondition())
				{
					worker = applyQueue[i];
					applyQueue.RemoveAt(i); i--;
				}
				if (worker==null) break; //no suitable threads

				//apply
				worker.stage = Stage.applyRunning;
				worker.apply();
				worker.stage = Stage.idle; 

				applyNow++;
			}
		}

		public Action generate;
		public Action apply;

		Thread thread;
		object locker = new object();

		public enum Stage { idle, threadEnqueued, threadRunning, applyEnqueued, applyRunning }; //threadComplete=applyEnqueued, complete=idle, applyRunning if access from other non-main thread
		public Stage stage = Stage.idle;
		public bool stop = false;
		
		public Func<bool> threadCondition; //will not start thread unless condition is met
		public Func<bool> applyCondition; //will not start apply unless condition is met

		//public int priority = 10;

		public void Stop ()
		{
			stop = true;

			//if in queue - remove
			if (threadsQueue.Contains(this)) threadsQueue.Remove(this);
			if (applyQueue.Contains(this)) applyQueue.Remove(this);
		}

		public void Start ()
		{
			//Before debugging why generate does not start at all make sure Threader.Refresh() is called in Update

			Stop();

			threadsQueue.Add(this);
			stage = Stage.threadEnqueued;
		}
		public void Start (Action generate, Action apply) { this.generate = generate; this.apply = apply; Start(); }

		private void ThreadFn ()
		{
			stop = false;

			//generating
			if (generate != null) lock (locker)
			{
				try { generate(); }
				catch (System.Exception e) { Debug.LogError("Thread Error: " + e); }
			}

			//enqueuing apply
			if (!stop) //if not stopped
			{
				applyQueue.Add(this);
				stage = Stage.applyEnqueued;
			}

			//decreasing the number of running threads
			threadsRunning--;
			
		}

		private void ApplyFn () 
		{
			if (apply != null)
			{
				try { generate(); }
				catch (System.Exception e) { Debug.LogError("Apply Error: " + e); }
			}
		}


		public void SpinFor (ThreadWorker otherWorker, int waitTime = 10, bool waitForApply=false)
		{
			//debug thread order
			if (otherWorker.stage==Stage.threadEnqueued)
			{
				Debug.Log("Trying to spin for enqueued thread. Ignoring spin.");
				return;
			}

			//spinning
			if (waitForApply)
				while (otherWorker.stage!=Stage.idle) Thread.Sleep(waitTime);
			else
				while (otherWorker.stage==Stage.threadRunning) Thread.Sleep(waitTime);
		}

		public static bool IsMainThread
		{get{
			return mainThread==null || mainThread.Equals(Thread.CurrentThread);
		}}
	}
}