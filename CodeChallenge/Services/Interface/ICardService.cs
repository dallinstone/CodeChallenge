using CodeChallenge.Models;

namespace CodeChallenge.Services.Interface
{
    public interface ICardService
    {
        
        //Deal Card - put one card from top of deck into "out" pile
        public Card DealCard();

        //Shuffle - randomly order the cards in the "deck" pile
        public string Shuffle();

        //Discard - take one card from user (i.e. "out" pile) and put it into "discard" pile
        public string Discard(Card card);

        //Cut - cut the deck pile in the middle and stack the bottom on the top
        public string Cut(int location);

        //Order - order cards in deck in default order (leaving out and discard alone)
        public string Order();

        //Rebuild Deck - almost same as initializing deck, but only placing discard back in pile
        //      the "out" pile will still remain out
        public string RebuildDeck();

        //see comments on REbuild Deck in services
        public string RebuildDeckFull();

        //Cheat - see top card of deck but don't deal it 
        public Card Cheat();
    }
}