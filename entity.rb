class Entity
    attr_accessor :health :state
    #graphics reference
    #mask reference (ie physical shape of object for CD)
    #position

    def initialize windows
        BOX_SIZE = 10
        @window = window
        @state = :alive
        @body = CP::Body.new(10, 100)
        @body.p = CP::Vec2.new(50,50))
        @body.v = CP::Vec2.new(0,0)
        @body.a = (3 * Math::PI / 2.5)

        @shape_verts = [
            CP::Vec2.new(-BOX_SIZE, BOX_SIZE),
            CP::Vec2.new(BOX_SIZE, BOX_SIZE),
            CP::Vec2.new(BOX_SIZE, -BOX_SIZE),
            CP::Vec2.new(-BOX_SIZE, -BOX_SIZE),
        ]

        @shape = CP::Shape::Poly.new(@body,
                                     @shape_verts,
                                     CP::Vec2.new(0,0))

        @shape.e = 0
        @shape.u = 1 

        @window.space.add_body(@body)
        @window.space.add_shape(@shape)
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
