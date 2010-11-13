TARGET = GameWindow

all: GameWindow.rb
	ruby $(TARGET).rb

%Test: %Test.rb
	ruby 

clean: 
	rm *~
