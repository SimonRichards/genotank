require 'gosu'
include Gosu
include Math
load 'interval.rb'

class Projectile
    attr_reader(:momentum,:position)
    def initialize(x, y, angle, mass, velocity, window)
        @x = x
        @x0 = x
        @y = y
        @y0 = y
        @angle = angle
        @velocity = velocity
        @momentum = mass*velocity
        @projectile_image = Image.new window, "media/projectile.png", true
        @window = window
    end

    def update
        @x0 = @x
        @y0 = @y
        @x += $DT * @velocity * Math.sin(@angle*2*Math::PI / 360)
        @y -= $DT * @velocity * Math.cos(@angle*2*Math::PI / 360)
    end

    def draw
        @projectile_image.draw @x, @y, 5
    end

    def to_s #debugging only, DELETEME
        print "position:", @position
        print "velocity:", @velocity
        print "momentum:", @momentum
    end

    def obstructed?
        @window.map.at(@x, @y)==  '#'
    end

    def hit? o
        if @x > o.x - o.width/2 && @x < o.x + o.width/2 && @y > o.y - o.width/2 && @y < o.y + o.height/2 
            return true
        else
            return false
        end


        this_x_range = Interval.new @x, @x0
        this_y_range = Interval.new @y, @y0
        object_x_range = Interval.new object.x, object.x + object.width 
        object_y_range = Interval.new object.y, object.y + object.height
        i1 = this_x_range.intersection object_x_range
        if i1 != nil
            i2 = this_y_range.intersection object_y_range
            if i2 != nil 
                if (i1.intersection i2) != nil
                    return true
                end
            end
        end    
        return false
    end

    def on_screen
        @x > 0 && @x < $WIDTH && @y > 0 && @y < $HEIGHT
    end

    def to_radians (x)
        x*2*Math::PI / 360
    end
end
