#!/usr/bin/ruby
require 'rubygems'
require 'gosu.rb'
#include Gosu
require 'player.rb'
require 'grunt.rb'
require 'actor.rb'
require 'map.rb'
require 'menu.rb'

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
        self.caption = "Tank"
        @menu = Menu.new self, ["Play", "Map Editor", "Exit"], [:play, :map_editor, :exit]
        @map = Map.new self, 'fun_map.map'
        @state = :menu
        @player = Player.new(rand($WIDTH), rand($HEIGHT), 1.0, self) 
        @player = Player.new(rand($WIDTH), rand($HEIGHT), 1.0, self)
        @grunts = Array.new 20 do |i| 
            Grunt.new rand($WIDTH), rand($HEIGHT), 1.0, self
        end
    end



    def button_down (id)
        self.close if id==KbEscape
        case @state 
        when :menu
            case id
            when KbDown
                @menu.down
            when KbUp
                @menu.up
            when KbReturn
                @state = @menu.enter   
            end
        when :paused
        when :in_game
        end		
    end

    def update
        case @state 
        when :menu
        when :paused
        when :play
            #    @map.update
            @grunts.each do |grunt| 
                if grunt.alive?
                    grunt.update [@player] 
                else
                    @grunts.delete(grunt)
                end
            end
        when :exit
            self.close
        end		
        @player.update @grunts
    end

    def draw
        case @state 
        when :menu
            @menu.paint
        when :paused

            #TODO Print each element from menu array with menuIndex element highlighted			
        when :play
            @map.draw
            @grunts.each { |grunt| grunt.draw }
            @player.draw
            #TODO print all on-screen entities, terrain and HUD
        end		
    end
end

g = Game.new
g.show
