class Tree
    attr_accessor :head

    def initialize
        @head = Node.new :*, Node.new(3), Node.new(7) 
    end

    def evaluate input
        @head.evaluate   
    end


    def insert node
    end


    class Node
        attr_accessor :l, :r, :val
        def initialize val, l = nil, r = nil
            @l, @r, @val = l, r, val
        end

        def evaluate
            if @l == nil
                @val
            else
                @l.evaluate.send @val, @r.evaluate
            end
        end
    end
end
