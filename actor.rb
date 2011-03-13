require '/var/lib/gems/1.8/gems/gosu-0.7.27.1/lib/gosu'
#require 'gosu'
include Gosu
load 'projectile.rb'

class Actor
    attr_reader :mass, :size, :velocity, :pMutate, :x, :y, :width, :height
    def to_radians (x)
        x*2*Math::PI / 360
    end
    
    def alive?
      if @health == 100
        return true
      else
        return false
      end
    end
        
    def initialize(x, y, angle, window)
        @game = window
        @width = 40
        @height = 40

        @x = x
        @y = y
        
        while obstructed?
            @x = rand $WIDTH
            @y = rand $HEIGHT
        end

        @health = 100
        @t = {:bomb => 5000, :bullet => 500}
        @angle = angle
        @turret_angle = angle
        @v = 0.0
        @mass = 50
        @acceleration = 1
        @turn_speed = 3
        @projectile_mass = 50
        @muzzle_velocity = 50
        @chassis = Image.new window, "media/tank-chassis.png", true
        @turret = Image.new window, "media/tank-turret.png", true
        @cursor = Image.new window, "media/cursor.png", true
        @projectiles = Array.new
        @bombs = Array.new 360
        
        @reload_timers = Hash.new(-1000)       
      
        @last_millis = milliseconds
      
        @dir = :none
        @accel = :none
    end
       
    def reload(id)
      if milliseconds - @reload_timers[id] > @t[id]
        @reload_timers[id] = milliseconds
        return true
      else
        return false
      end
    end
    
    def fire
        #self.impulse(-@turret_angle, @projectile_mass * @muzzle_velocity)
        @projectiles.push Projectile.new @x, @y, @turret_angle, @projectile_mass, @muzzle_velocity, @game if reload(:bullet)
    end
    
    
    def impulse angle, input_momentum
        @vx =  ( @vx*@mass+(input_momentum * Math.cos(angle)) ) / @mass
        @vy =  ( @vy*@mass+(input_momentum * Math.sin(angle)) ) / @mass
    end
        
    def turn(dir)
      if dir == :cw
          @angle += @turn_speed
      elsif dir == :ccw
          @angle -= @turn_speed
      end
    end

    def accelerate 
        @v += @acceleration
    end

    def brake
        if @v > 0
            @v -= @acceleration*2  unless @v < 0 
        else 
            @v -= @acceleration/2  
        end
    end

    def friction
        @v -= @v*$K_F
    end

    def damage momentum 
        @health -= momentum / 10
    end   

    def obstructed?
        @game.map.at(@x+@width/2, @y+@height/2) == '#'  ||
        @game.map.at(@x+@width/2, @y-@height/2) == '#'  ||
        @game.map.at(@x-@width/2, @y+@height/2) == '#'  ||
        @game.map.at(@x-@width/2, @y-@height/2) == '#'
    end


    def update others
        friction
        controls
        @projectiles.each_with_index  do |p, i| 
            if p.on_screen 
                p.update 
                if p.obstructed?
                    @projectiles.delete_at i
                    next
                end
                others.each do |other|
                    if p.hit? other
                        other.damage p.momentum
                        @projectiles.delete_at i
                    end
                end
            else
                @projectiles.delete_at i
            end
        end
        @projectiles.compact!

        @v = -@v if obstructed?

        @x += @v*$DT*Math.sin(to_radians @angle)
        @y -= @v*$DT*Math.cos(to_radians @angle)

        if @x < 0 then @x += $WIDTH 
        elsif @x > $WIDTH then @x -= $WIDTH end
        if @y < 0 then @y += $HEIGHT 
        elsif @y > $HEIGHT then @y -= $HEIGHT end
    end

    def draw
        @projectiles.each {|p| p.draw}
        @chassis.draw_rot @x, @y, $Z_TANK, @angle
        @turret.draw_rot @x, @y, $Z_TANK , @turret_angle
        @cursor.draw @game.mouse_x - 5, @game.mouse_y - 5, 10
    end
end

