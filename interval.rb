class Interval
    attr_accessor :a, :b
    def initialize a, b
        if a < b
            @a = a
            @b = b
        else
            @a = b
            @b = a
        end    
    end

    def max a, b
        a > b ? a : b
    end

    def min a, b
        a < b ? a : b
    end

    def intersection(i)
        if i == nil
            return nil
        elsif i.a > @b || i.b < @a 
            return nil 
        else
            return Interval.new(max(@a,i.a), min(@b,i.b)) 
        end
    end

end
