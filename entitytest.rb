require 'rubygems'
require 'test/unit'
require 'entity'
class EntityTest < Test::Unit::TestCase
    def setup
    end

    def teardown
    end

    def test_death
        testEntity = Entity::new
        testEntity.hit(100)
        assert(testEntity.state == State::DEAD, 'Kill the tank and check it dies')
    end

end


