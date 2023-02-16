using System;
using System.Diagnostics;
using System.Threading;

class Counter
{
	const int NUMBERS          = 20;

	const int LOOP_COUNT       = 100000;
	const int REPEAT_PER_CALL  = 100;
	const int THREAD_COUNT     = 12; // 실행 대상 컴퓨터의 Core(Thread) 수에 맞춰서 값을 설정하시오.
	                                 // 예) 6 core * 2 threads 인 경우 12 값 설정
	                                 // 댠, 21(포함) 을 넘지 않도록 설정하시오. 즉, TOTAL 값이 2.1억을 넘지 않도록 하시오.

	const int TOTAL            = LOOP_COUNT * THREAD_COUNT * REPEAT_PER_CALL;

	/********************************************************************
	 *                                                                  *
	 *      main                                                        *
	 *                                                                  *
	 ********************************************************************/

	static public void Main(String[] args)
	{
		CounterStorage storage = new CounterStorage(NUMBERS);

		// prepare threads

		Thread[] threads = new Thread[THREAD_COUNT];

		for (int threadIndex = 0; threadIndex < threads.Length; threadIndex++) {
			threads[threadIndex] = new Thread(ThreadProc);
		}

		// start threads

		Stopwatch stopwatch = Stopwatch.StartNew();

		for (int threadIndex = 0; threadIndex < threads.Length; threadIndex++) {
			threads[threadIndex].Start(storage);
		}

		// join threads

		for (int threadIndex = 0; threadIndex < threads.Length; threadIndex++) {
			threads[threadIndex].Join();
		}

		long elapsed = stopwatch.ElapsedMilliseconds;

		// print

		Console.WriteLine("elapsed = " + elapsed.ToString());
		storage.Print();
	}

	/********************************************************************
	 *                                                                  *
	 *      THREAD                                                      *
	 *                                                                  *
	 ********************************************************************/

	static void ThreadProc(object argument)
	{
		CounterStorage storage = (CounterStorage) argument;

		for (int loop = 0; loop < LOOP_COUNT; loop++) {
			for (int number = 0; number < NUMBERS; number++) {
				storage.Increase(number, REPEAT_PER_CALL);
			}
		}
	}

	/********************************************************************
	 *                                                                  *
	 *      STORAGE : 수정 대상                                         *
	 *                                                                  *
	 ********************************************************************/

	class CounterStorage
	{
		private volatile int[] myCounter;

		public CounterStorage(int numbers)
		{
			myCounter = new int[numbers];
		}

		public void Increase(int number, int repeat)
		{
			for (int i = 0; i < repeat; i++) {
				myCounter[number]++;
			}
		}

		public void Print()
		{
			for (int i = 0; i < myCounter.Length; i++) {
				Console.WriteLine(myCounter[i]);
			}
		}
	}
}
