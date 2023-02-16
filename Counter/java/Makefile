all: Counter.class

Counter.class: Counter.java
	@ javac -encoding utf-8 Counter.java

run: Counter.class
	@ java Counter

clean:
	@ rm -rf *.class

