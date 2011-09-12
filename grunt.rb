class Grunt < Actor
  
  def initialize x, y, angle, window
      super x, y, angle, window
      @last_update = milliseconds
      @next_update = rand(500)
      @d_turret = 0
      @player = window.player
  end
  
  
  def controls

    track_player
      if milliseconds - @last_update > @next_update
        
          a, b = rand, rand
          if a > 0.9 then @dir = :cw elsif a > 0.8 then @dir = :ccw else @dir = nil end
          if b > 0.75 then @accel = :forward elsif b > 0.5 then @accel = :forward else @accel = nil end

          #fire unless rand > 0.2
          @last_update = milliseconds
          @next_update = rand(500)
      end

      if @dir 
       turn @dir 
      end

      if @accel  
          if @accel == :forward
              accelerate
          else
              brake
          end
      end
  end

  private

  # accelerate
  # brake
  # distance to enemy
  # turn left
  # turn right
  # fuel stuff?
  # communication
  
  def track_player
    a = (@turret_angle - (angle @x, @y, @player.x, @player.y))% 360
    @turret_angle += if a > 180 then 1 elsif a == 180 then 0 else -1 end
  end
    
    
end
