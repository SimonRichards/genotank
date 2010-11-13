class Entity
    attr_accessor :health :state # makes getters and setters automagically
    #graphics reference
    #mask reference (ie physical shape of object for CD)
    #position
    
    def initialize
        @state = State::ALIVE
    end

    def hit(damage)
        health -= damage
    end

    def update
        if health <= 0
            @state = State::DEAD
        end
    end
end

