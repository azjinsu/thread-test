public class Counter extends Thread {
	static final int NUMBERS          = 20;

	static final int LOOP_COUNT       = 100000;
	static final int REPEAT_PER_CALL  = 100;
	static final int THREAD_COUNT     = 12; // 실행 대상 컴퓨터의 Core(Thread) 수에 맞춰서 값을 설정하시오.
	                                        // 예) 5 core * 12 threads 인 경우 8 값 설정
	                                        // 댠, 21(포함) 을 넘지 않도록 설정하시오. 즉, TOTAL 값이 2.1억을 넘지 않도록 하시오.

	static final int TOTAL            = LOOP_COUNT * THREAD_COUNT * REPEAT_PER_CALL;

	/********************************************************************
	 *                                                                  *
	 *      main                                                        *
	 *                                                                  *
	 ********************************************************************/

	public static void main(String[] args) {
		CounterStorage storage = new CounterStorage(NUMBERS);

		// prepare threads

		Counter[] threads = new Counter[THREAD_COUNT];
		for (int threadIndex = 0; threadIndex < threads.length; threadIndex++) {
			threads[threadIndex] = new Counter(storage);
		}

		// start threads

		long start = System.currentTimeMillis();

		for (int threadIndex = 0; threadIndex < threads.length; threadIndex++) {
			threads[threadIndex].start();
		}

		// join threads

		for (int threadIndex = 0; threadIndex < threads.length; threadIndex++) {
			try {
				threads[threadIndex].join();
			} catch (Exception e) {
			}
		}

		long finish = System.currentTimeMillis();

		// print

		System.out.println("elapsed = " + Long.toString(finish - start));
		storage.Print();
	}

	/********************************************************************
	 *                                                                  *
	 *      THREAD                                                      *
	 *                                                                  *
	 ********************************************************************/

	private CounterStorage myStorage;

	public Counter(CounterStorage storage) {
		myStorage = storage;
	}

	@Override
	public void run() {
		for (int loop = 0; loop < LOOP_COUNT; loop++) {
			for (int number = 0; number < NUMBERS; number++) {
				myStorage.Increase(number, REPEAT_PER_CALL);
			}
		}
	}

	/********************************************************************
	 *                                                                  *
	 *      STORAGE : 수정 대상                                         *
	 *                                                                  *
	 ********************************************************************/

	static class CounterStorage {
		private volatile int[] myCounter;

		public CounterStorage(int numbers) {
			myCounter = new int[numbers];
		}

		public void Increase(int number, int repeat) {
			for (int i = 0; i < repeat; i++) {
				myCounter[number]++;
			}
		}

		public void Print() {
			for (int i = 0; i < myCounter.length; i++) {
				System.out.println(myCounter[i]);
			}
		}
	}
}
