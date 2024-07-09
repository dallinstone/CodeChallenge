using CodeChallenge.Services.Interface;
using CodeChallenge.Models;

namespace CodeChallenge.Services.Services
{
    public class CardServices : ICardService
    {
        //define our list of card values and suits in order
        private static readonly string[] Suits = { "Spades♠️", "Hearts♥️", "Clubs♣️", "Diamonds♦️" };
        private static readonly string[] Cards = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        private List<Card> _deck;
        //I know this wasn't a requirement but I felt weird about not keeping track of the cards that aren't in
        //my deck or my discard
        private List<Card> _out;
        private List<Card> _discard;


        //initialize the service -- mostly setting our "deck" variable to the ordered deck
        public CardServices()
        {
            _deck = InitializeDeck();
            _out = new List<Card>();
            _discard = new List<Card>();
        }

        //A separate method for initializing the deck, since we can reuse this for both rebuild and order
        //The other way I considered doing this was creating another "DefaultDeck" ordered list and then when 
        //      I needed to use it just setting "_deck = _baseDeck" or something like that, but I think this is 
        //      fine and similar in performance, at least for this simple case. 
        //Another option I considered was assigning the cards to an enum so that they would have a number value 
        //      but that sounded clunky and like not fun to write. Possibly a bit more efficient (since ordering ints is
        //      easier than regenerating the entire deck) but not enough to worry about for this task
        private List<Card> InitializeDeck()
        {
            var initDeck = new List<Card>();
            //since our suits and cards are in order, this will always return the deck in the correct order
            foreach (string suit in Suits)
            {
                foreach (string card in Cards)
                {
                    initDeck.Add(new Card { Suit = suit, Val = card });
                }
            }
            return initDeck;
        }

        //Deal Card 
            //Deals one card from the top of the deck
            //There is no need to track who the card was dealt to
        public Card DealCard()
        {
            //first do a bit of simple error handling 
            if (_deck.Count == 0) throw new InvalidOperationException("No cards in deck");

            //pull the first card from the deck, put it into _out, and remove it from _deck
            var card = _deck.First();
            _out.Add(card);
            _deck.RemoveAt(0);
            return card;
        }

        //Shuffle
            //Randomize all the cards remaining in the deck. Do not place the discard pile back into the deck.
        public string Shuffle()
        {
            if (_deck.Count()==0) return "No cards in deck!";
            //adding try catch in case the shuffle fails for some reason
            //would probably fix this for a production app, but this is fine for now
            try
            {
                //create rng 
                var rnd = new Random();
                //order the _deck by a random value 
                _deck = _deck.OrderBy(c => rnd.Next()).ToList();
                return "Deck Shuffled";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //Discard
            //This will allow a card player to place a dealt card into the discard pile.
        public string Discard(Card discarded)
        {
            //make sure that the discarded card is in the _out pile so that they're not sending back 
            //a card that doesn't exist or something like that
            Card card = _out.Find(x => x.Suit == discarded.Suit && x.Val == discarded.Val);
            if (card == null)
            {
                return "You didn't have that card in your deck to discard, cheater!";
            }
            _out.Remove(card);
            _discard.Add(card);
            return "Your card has been discarded";
        }

        //Cut
            //Specify a location to split the deck of cards into two and put the bottom half on top of the top half.
        public string Cut(int cut)
        {
            //first make sure that our cut spot is valid -- i.e. it's higher than 0 and less than the size of the deck
            if (cut <= 0)
            {
                return "Please pick a number that's possible to have in the deck.";
            }
            if (cut > 52)
            {
                return "There should never be more than 52 cards in the deck. Unless you're cheating.";
            }
            if (cut > _deck.Count)
            {
                return "There are not that many cards remaining in the deck.";
            }

            //take the first x cards from the deck
            List<Card> top = _deck.Take(cut).ToList();

            //take the remainder of the cards
            _deck = _deck.Except(top).ToList();

            //put the top of the deck on the bottom
            _deck.AddRange(top);

            return "Deck has been cut!";
        }

        //Order
            //Order all the remaining cards in the deck into the default order
            //Leave the cards in the discard pile where they are
        public string Order()
        {
            if (_deck.Count()==0) return "No cards in deck!";
            try {
                //reinitialize the deck, but exclude any cards that are currently dealt or discarded
                //the requirement don't explicitly call out the "_out" cards but I want to preclude getting 
                //duplicate cards putting back in the deck (or else the Cut method won't work right)
                _deck = InitializeDeck().Except(_discard).Except(_out).ToList();
                return "Deck ordered successfully.";
            }
            catch (Exception ex) {
                return String.Format("There was an error ordering your deck: {0}", ex);
            }
        }

        //Rebuild Deck
            //Place the discard pile back into the deck and order the deck using the standard default order.
        public string RebuildDeck()
        {
            
            if (_deck.Count()==0 && _discard.Count()==0) return "No cards in deck or discard!";
            try {
                //ok so I went back and forth on this -- it makes sense that "rebuild deck" would completely rebuild
                //the deck from scratch (i.e. it would be a complete start over), but it explicitly states "Put the 
                //Discard pile back into the deck" and doesn't say anything about returning the currently dealt cards.
                //To show that I know how to do it the other way I've added a "RebuildDeckFull" method as well
                //that will completely reset the deck including taking back all dealt cards
                _deck = InitializeDeck().Except(_out).ToList();
                _discard.Clear();
                return "Discard pile added back to deck, and deck ordered.";
            }
            catch (Exception ex) {
                return String.Format("There was an issue with your deck rebuild: {0}", ex);
            }
        }

        public string RebuildDeckFull()
        {
            try {
                _deck = InitializeDeck();
                _discard.Clear();
                _out.Clear();
                return "Deck Initialized Fully";
            }
            catch (Exception ex) {
                return String.Format("Error rebuilding deck: {0}", ex);
            }
            
        }

        //Cheat
            //It allows a player to peak at the next card without dealing with it.
        public Card Cheat()
        {
            //this one is really straightforward. 
            if (_deck.Count() == 0) throw new InvalidOperationException("No cards remain in deck!");
            return _deck.First();
        }
    }
}