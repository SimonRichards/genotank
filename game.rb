require '/var/lib/gems/1.8/gems/gosu-0.7.27.1/lib/gosu' #TODO fix path
#require 'gosu'
include Gosu
load 'player.rb'
load 'grunt.rb'
load 'actor.rb'
load 'map.rb'

class Game < Window
    attr_accessor :player,:map 
    $\ = "\n"
    $DT = 0.1
    $Z_BG = 0
    $Z_TANK = 1
    $K_F = 0.05

    @selection_index = 0
    def initialize
        super  screen_width, screen_height, true, 10 #640, 480, false, 10 #
        $WIDTH, $HEIGHT = screen_width, screen_height # 640, 480  #
        #self.caption = "Tank"
        @menu_text = [['play', 'options', 'exit'], ['stuff', 'junk', 'moreCrap'], ['resume', 'quit']]
        @menu_index = 0
        @line_index = 0
        @map = Map.new self, 'fun_map.map'
        @state = :menu
        @player = Player.new(rand($WIDTH), rand($HEIGHT), 1.0, self) #TODO replace with do-while
        @player = Player.new(rand($WIDTH), rand($HEIGHT), 1.0, self)
        @grunts = Array.new 20 do |i| 
            Grunt.new rand($WIDTH), rand($HEIGHT), 1.0, self
        end
    end


    # Rewrite all of this with a fancy set of classes
    def menu_callback
        case @menu_index
        when 0
            case @line_index
            when 0
                @state = :in_game
            when 1 
                @menu_index = 1
            when 2
                self.close
            end
        when 1
            case @line_index
            when 0
                print 'stuff'
            end
        when 2
            case @line_index
            when 0
                @state = :in_game
            when 1
                self.close
            end
        end
    end



    def button_down (id)
        self.close if id==KbEscape
        case @state 
        when :menu
            case id
            when KbDown
                @line_index += 1 if @line_index < @menu_text[@menu_index].length
            when KbUp
                @line_index -= 1 if @line_index > 0
            when KbReturn
                menu_callback    
            end
        when :paused
        when :in_game
        end		
    end

    def update
        case @state 
        when :menu
        when :paused
        when :in_game
        #    @map.update
            @grunts.each do |grunt| 
                if grunt.alive?
                    grunt.update [@player] 
                else
                    @grunts.delete(grunt)
                end
            end
        end		
        @player.update @grunts
    end

    def draw
        case @state 
        when :menu
        when :paused

            #TODO Print each element from menu array with menuIndex element highlighted			
        when :in_game
            @map.draw
            @grunts.each { |grunt| grunt.draw }
            @player.draw
            #TODO print all on-screen entities, terrain and HUD
        end		
    end
end

g = Game.new
g.show
