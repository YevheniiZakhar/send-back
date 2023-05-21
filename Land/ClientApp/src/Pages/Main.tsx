import React from 'react';
import AdCard from '../Components/AdCard/AdCard';

interface IData {
  id: number;
  title: string;
  description: string;
  currency: string;
  price: number;
  image: string;
}

const Main = () => {
  const [data, setData] = React.useState<IData[]>([]);

  React.useEffect(() => {
    setData([
      {
        id: 1,
        title: '1 Lorem ipsum dolor sit.',
        description:
          'lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.',
        currency: '$usd',
        price: 1,
        image: 'https://picsum.photos/800/600',
      },
      {
        id: 2,
        title: '2 Lorem ipsum dolor sit amet consectetur.',
        description:
          'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.',
        currency: '$usd',
        price: 1,
        image: 'https://picsum.photos/800/600',
      },
      {
        id: 3,
        title: '3 Lorem, ipsum.',
        description:
          'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.',
        currency: '$usd',
        price: 1,
        image: 'https://picsum.photos/800/600',
      },
      {
        id: 4,
        title: '4 Lorem ipsum dolor sit amet.',
        description:
          'lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.',
        currency: '$usd',
        price: 1,
        image: 'https://picsum.photos/800/600',
      },
    ]);
  }, []);

  return (
    <div
      style={{
        display: 'grid',
        gridAutoRows: '1fr',
        gridTemplateColumns: 'repeat(auto-fill, minmax(360px, 1fr))',
        gap: '1rem',
        alignItems: 'center',
        justifyItems: 'center',
      }}
    >
      {data.map((value: IData) => (
        <AdCard
          id={value.id}
          currency={value.currency}
          description={value.description}
          title={value.title}
          image={value.image}
          price={value.price}
          key={value.id}
        />
      ))}
    </div>
  );
};

export { Main };
