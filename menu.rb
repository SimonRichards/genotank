require '/var/lib/gems/1.8/gems/gosu-0.7.27.1/lib/gosu' #TODO fix path
#require 'gosu'
include Gosu
class Menu
    def initialize names, callbacks
        @names = names
        @callbacks = callbacks
        @index = 0;
    end

    def paint
        @names.each_value do |item|
            item.paint
        end
        @names[@index].each_with_index do |string, i|
            colour = case i
                     when @line_index then 0xff5ccccc
                     else 0xffffffff
                     end
            image = Image.from_text self, string, default_font_name, 50
            image.draw $WIDTH/3, i*100+200, 9, 1, 1, colour
        end
    end

    def up
        if @index > 0
            @index -= 1
        end
    end

    def down
        if @index < @names.length - 1
            @index += 1
        end
    end

    def enter
        @callbacks[@index]
    end
end


