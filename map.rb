class Map
    def initialize(window, *args)  
        if args.size > 2 
            raise 
        else  
            if args.size == 1  
                generate_map_from args[0]
            else  
                generate_random args[0..2]
            end  
        end  
        @dirt = Image.new window, "media/dirt.png", true
    end

    def at x,y
        x = x.to_i
        y = y.to_i
        x /= 60
        y /= 60
        x %= 32
        y %= 18
        @elements[y][x]
    end

    def generate_map_from file
        f = File.new('media/' + file, "r")
        map_buffer = Array.new(100)
        f.each_line do |line| 
            map_buffer[f.lineno] = line
        end
        map_buffer.compact!
        @elements=Array.new 18 do |i|
            Array.new 32 do |j|
                map_buffer[i][j].chr if map_buffer[i]
            end
        end
    end


    def draw
        @elements.length.times do |i|
            @elements[i].length.times do |j|
                if @elements[i][j]  == '#'
                    @dirt.draw j*60, i*60, 3
                end
            end
        end
    end

end
