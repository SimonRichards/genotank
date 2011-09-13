class Nature

    def reproduce (parentA, parentB)
        offspring = parentA.new
        parentA.genome.each_key do |gene|
            if (parentA.pMutate + parentB.pMutate)/2 > rand
                offspring[gene] = self.mutate
            else
                offspring[gene] = self.crossover(parentA.genome[gene], parentB.genome[gene])
            end
        end
    end

    def mutate
        rand(100)
    end

    def crossover(geneA, geneB)
        (geneA + geneB)/2
    end

end

