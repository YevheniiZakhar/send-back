import AspectRatio from '@mui/joy/AspectRatio';
import Box from '@mui/joy/Box';
import Button from '@mui/joy/Button';
import Sheet from '@mui/joy/Sheet';
import Typography from '@mui/joy/Typography';
import { Card } from '@mui/material';
import * as React from 'react';

interface IAdCardProps {
  id: number;
  title: string;
  description: string;
  image: string;
  price: number;
  currency: string;
}

export default function AdCard(prop: IAdCardProps) {
  const { title, description, image, price, currency } = prop;

  const [isHovered, setIsHovered] = React.useState(false);
  const onMouseEnter = () => {
    console.log('onMouseEnter');
    setIsHovered(true);
  };
  const onMouseLeave = () => {
    console.log('onMouseLeave');
    setIsHovered(false);
  };

  return (
    <Box
      sx={{
        display: 'flex',
        p: 2,
        py: 2,
        borderRadius: 'xs',
      }}
    >
      <Card
        sx={{
          alignSelf: 'center',
          maxWidth: '100%',
          minWidth: { xs: 220, sm: 360 },
          mx: 'auto',
          boxShadow: 'sm',
          borderRadius: 'md',
          overflow: 'auto',
          transition: '0.15s ease-in-out',
          transitionDelay: isHovered ? '500ms' : '0ms',
          transform: isHovered ? 'scale3d(1.3, 1.3, 3)' : 'scale3d(1, 1, 1)',
          zIndex: isHovered ? 1 : 0,
        }}
        onMouseEnter={onMouseEnter}
        onMouseLeave={onMouseLeave}
        raised={isHovered}
      >
        <Sheet
          sx={{
            borderWidth: '0 0 1px 0',
            display: 'flex',
            alignItems: 'center',
            p: 2,
            borderBottom: '1px solid',
            borderColor: 'background.level2',
          }}
        >
          <Typography level="h2" fontSize="md">
            {/* Title of the Ad Card */}
            {title}
          </Typography>
        </Sheet>
        <Sheet sx={{ p: 2 }}>
          <Sheet
            variant="outlined"
            sx={{
              borderRadius: 'md',
              overflow: 'auto',
              borderColor: 'background.level2',
              bgcolor: 'background.level1',
            }}
          >
            <AspectRatio>
              <img alt="" src={image} />
            </AspectRatio>
            <Box
              sx={{
                display: 'flex',
                p: 1.5,
                gap: 1.5,
                '& > button': { bgcolor: 'background.body' },
              }}
            >
              <Typography level="h5" fontSize="md" flexWrap="wrap">
                {/* some description of the ad card here */}
                {description}
              </Typography>
            </Box>
          </Sheet>
        </Sheet>
        <Sheet
          sx={{
            display: 'flex',
            p: 2,
            borderTop: '1px solid',
            borderColor: 'background.level2',
            justifyContent: 'space-between',
          }}
        >
          <Button size="md" variant="plain">
            See more
          </Button>
          <Button
            color="success"
            size="md"
            style={{
              gap: 3,
              padding: '0.5rem 1rem',
            }}
          >
            <Typography level="h3" fontSize="md" textColor="common.white">
              {price} {currency}
            </Typography>
            <Typography level="h4" fontSize="sm" textColor="common.white">
              Add to cart
            </Typography>
            {/* Remove comment below to check  */}
            {/* <ShoppingCartCheckoutIcon /> */}
          </Button>
        </Sheet>
      </Card>
    </Box>
  );
}
