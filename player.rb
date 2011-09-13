load 'actor.rb'

class Player < Actor
    def controls
        fire if @game.button_down? MsLeft
        @turret_angle = angle @x, @y, @game.mouse_x, @game.mouse_y
        @ax, @ay = 0, 0
        accelerate if @game.button_down? KbW
        brake if @game.button_down? KbS 
        turn :ccw if @game.button_down? KbA
        turn :cw if @game.button_down? KbD
    end

end

